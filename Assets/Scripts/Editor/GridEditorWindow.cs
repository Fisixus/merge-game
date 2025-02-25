using System;
using System.Linq;
using Core.GridSerialization;
using DI.Contexts;
using MVP.Helpers;
using MVP.Models;
using MVP.Models.Interface;
using MVP.Presenters;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    public class GridEditorWindow : EditorWindow
    {
        private int _gridWidth = 8;
        private int _gridHeight = 8;
        private JsonPawn[,] _gridObjects;
        
        private Vector2 _scrollPosition = Vector2.zero; //  Store scroll position

        [MenuItem("Tools/Grid Editor")]
        public static void ShowWindow()
        {
            GetWindow<GridEditorWindow>("Grid Editor");
        }

        private void OnEnable()
        {
            InitializeGrid();
        }

        private void OnGUI()
        {
            DrawHeader();
            DrawGridControls();
            DrawGridTable();
            DrawLoadGridButton();
            DrawApplyChangesButton();
        }

        #region Header and Controls

        private void DrawHeader()
        {
            GUILayout.Label("Grid Editor", EditorStyles.boldLabel);
        }

        private void DrawGridControls()
        {
            int newGridWidth = EditorGUILayout.IntField("Grid Width", _gridWidth);
            int newGridHeight = EditorGUILayout.IntField("Grid Height", _gridHeight);

            if (newGridWidth != _gridWidth || newGridHeight != _gridHeight)
            {
                ResizeGrid(newGridHeight, newGridWidth);
                _gridWidth = newGridWidth;
                _gridHeight = newGridHeight;
            }
        }


        private void DrawGridTable()
        {
            EditorGUILayout.LabelField("Pawns Table", EditorStyles.boldLabel);

            if (_gridObjects == null)
            {
                Debug.LogWarning("Pawns not initialized. Initializing with default values.");
                InitializeGrid();
            }

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.Height(300)); //  Add horizontal scrolling

            for (int i = 0; i < _gridHeight; i++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int j = 0; j < _gridWidth; j++)
                {
                    JsonPawn pawn = _gridObjects[i, j];
                    if(pawn == null) continue;

                    EditorGUILayout.BeginVertical("box"); //  Set width per item

                    // Convert stored string to Enum for dropdown
                    JsonGridPawnType pawnTypeEnum = Enum.TryParse(pawn.pawn_type, out JsonGridPawnType parsedEnum) ? parsedEnum : JsonGridPawnType.empty;
                    pawnTypeEnum = (JsonGridPawnType)EditorGUILayout.EnumPopup("Type", pawnTypeEnum); //  Adjust width

                    // Convert back to string for storage in JsonPawn
                    pawn.pawn_type = pawnTypeEnum.ToString();
                    pawn.level = EditorGUILayout.IntField("Level", pawn.level); //  Adjust width

                    EditorGUILayout.EndVertical();

                    _gridObjects[i, j] = pawn;
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView(); //  Close scroll view
        }
        
        private void DrawLoadGridButton()
        {
            if (GUILayout.Button("Load Grid To Editor"))
            {
                LoadGrid();
            }
        }

        private void DrawApplyChangesButton()
        {
            if (GUILayout.Button("Apply Changes"))
            {
                ApplyChanges();
            }
        }

        #endregion

        #region Level Logic

        private void InitializeGrid()
        {
            if (_gridObjects == null)
            {
                _gridObjects = new JsonPawn[_gridHeight, _gridWidth];

                for (int i = 0; i < _gridHeight; i++)
                {
                    for (int j = 0; j < _gridWidth; j++)
                    {
                        _gridObjects[i, j] = new JsonPawn { pawn_type = JsonGridPawnType.empty.ToString(), level = 0 }; //  Default enum value as string
                    }
                }
            }
        }
        
        private void LoadGrid()
        {
            if (!IsMergeSceneActive())
            {
                Debug.LogWarning("You are not in the MergeScene!");
                return;
            }

            var context = SceneHelper.FindSceneContextInActiveScene();
            var gridModel = context.SceneContainer.Resolve<IGridModel>();
            var gridInfo = gridModel.GetGridInfo();

            if (gridInfo == null)
            {
                Debug.LogError($"Failed to load grid!");
                return;
            }

            // Update grid dimensions and move count
            _gridWidth = gridInfo.GridSize.y;
            _gridHeight = gridInfo.GridSize.x;

            // Convert grid data to JsonGridObjectType and update _gridObjects
            var gridData = GridSerializer.ConvertToJsonPawn(gridInfo.GridPawnTypes, gridInfo.GridPawnLevels);
            ResizeGrid(_gridHeight, _gridWidth);
            PopulateGrid(gridData);

            Debug.Log($"Grid loaded successfully.");
        }

        private void ApplyChanges()
        {
            var gridJson = GridSerializer.ConvertToGridJson(_gridWidth, _gridHeight, _gridObjects);
            var (gridObjectTypes, gridObjectLevels, gridPawnCapacities) = GridSerializer.ProcessGridJson(gridJson);
            FillCapacitiesToValue(gridPawnCapacities, 10);
            var gridInfo = new GridInfo(Transpose(gridObjectTypes), Transpose(gridObjectLevels), Transpose(gridPawnCapacities));
            Debug.Log($"Grid JSON created:\n{JsonUtility.ToJson(gridJson, true)}");

            CreateGrid(gridInfo);
        }

        private static void CreateGrid(GridInfo gridInfo)
        {
            if (!IsMergeSceneActive())
            {
                Debug.LogWarning("You are not in the MergeScene!");
                return;
            }

            var context = SceneHelper.FindSceneContextInActiveScene();
            var gridPresenter = context.SceneContainer.Resolve<GridPresenter>();
            
            gridPresenter.LoadFromGridEditor(gridInfo);

            Debug.Log("Grid created successfully.");
        }
        
        
        
        #endregion

        #region Grid Management

        private void ResizeGrid(int newHeight, int newWidth)
        {
            if (newHeight <= 0 || newWidth <= 0)
            {
                Debug.LogError("Invalid grid dimensions. Height and Width must be greater than 0.");
                return;
            }

            var newGrid = new JsonPawn[newHeight, newWidth];

            for (int i = 0; i < newHeight; i++)
            {
                for (int j = 0; j < newWidth; j++)
                {
                    if (i < _gridHeight && j < _gridWidth && _gridObjects != null)
                    {
                        newGrid[i, j] = _gridObjects[i, j]; //  Preserve existing values
                    }
                    else
                    {
                        newGrid[i, j] = new JsonPawn { pawn_type = JsonGridPawnType.empty.ToString(), level = 0 }; //  Default enum value as string
                    }
                }
            }

            _gridObjects = newGrid;
            _gridHeight = newHeight;
            _gridWidth = newWidth;
        }
        private void PopulateGrid(JsonPawn[,] gridData)
        {
            for (int i = 0; i < _gridHeight; i++)
            {
                for (int j = 0; j < _gridWidth; j++)
                {
                    _gridObjects[i, j] = gridData[j, i];
                }
            }
        }
        #endregion

        #region Serialization Helpers

        private static T[,] Transpose<T>(T[,] array)
        {
            int rows = array.GetLength(0);
            int cols = array.GetLength(1);
            T[,] transposedArray = new T[cols, rows]; //  Swap dimensions

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    transposedArray[j, i] = array[i, j]; //  Swap row & column
                }
            }

            return transposedArray;
        }


        private static bool IsMergeSceneActive()
        {
            return SceneManager.GetActiveScene().name == "MergeScene";
        }

        private void FillCapacitiesToValue(int[,] gridPawnCapacities, int value)
        {
            for (int x = 0; x < _gridHeight; x++)
            for (int y = 0; y < _gridWidth; y++)
                gridPawnCapacities[x, y] = value;
        }
        #endregion
    }
}