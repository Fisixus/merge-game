# 🧩 Merge Game (Travel Town-Inspired)

A **merge-based puzzle game** built in Unity using C#. Players merge items to create higher-level objects and complete tasks. The game features an **inventory system, level-based progression, and producer mechanics**.

---

## 📖 About the Project

This project was developed as a **case study** and is not intended for commercial use. It replicates the core mechanics of **Travel Town-style** merging games while implementing **clean code architecture** and efficient **design patterns**.

---

## 🛠️ Built With

- **Unity 2022.3.8**
- **C#**
- **Third-Party Libraries:**
  - `DoTween` – Tweening engine for smooth animations.
  - `UniTask` – Async/await utilities for better performance.
  - `Yellowpaper.SerializedDictionary` – Serializable dictionary support for Unity.
- **Custom Implementations:**
  - **Custom DI (Dependency Injection)** – Flexible and modular dependency management.

---

## 🧩 Architecture

This project follows the **Model-View-Presenter (MVP)** architecture to ensure:

✅ **Separation of Concerns** – Clear distinction between game logic and UI.  
✅ **Scalability** – Easy to expand with new mechanics.  
✅ **Maintainability** – Code remains clean and modular.  

### 🔹 Key Components

#### **1️⃣ Models**
- **Manage game data**, including:
  - **Grid structure** (8x8 board).
  - **Player inventory** and stored items.
  - **Task management system**.

#### **2️⃣ Views**
- **Handle UI interactions** and display game visuals.
- **Examples:**
  - **Game grid display**.
  - **Inventory UI**.
  - **Task UI**.

#### **3️⃣ Presenters**
- **Bridge between Models & Views**.
- Process **user input**, update models, and synchronize UI.
- **Examples:**
  - **InventoryPresenter** – Manages inventory interactions.
  - **MergePresenter** – Handles merging logic.
  - **TaskPresenter** – Tracks task progress.

#### **4️⃣ Handlers**
- **Abstract game logic** from Presenters.
- **Examples:**
  - **GridPawnFactoryHandler** – Handles factories.
  - **EffectHandler** – Handles visual effects.

#### **5️⃣ Factories**
- **Optimize object creation and pooling**.
- **Examples:**
  - **ApplianceFactory** – Creates and recycles appliances.
  - **ProducerFactory** – Manages producer generation.

---

## 🛠️ Features

### 🗺️ 8x8 Grid System
- Players can **move items by dragging**.
- Items should be **placed in the nearest empty cell**.
- **Game state is saved locally** after every operation.

### 📦 Inventory System
- **Players can store mergeable items** in inventory.
- Items can be **dragged and dropped back to the board**.
- **Inventory saves & loads using JSON**.
- Inventory has **unlimited space**.

### 🔄 Merge Mechanics
- **Merge 2 identical items** to create a **higher-level item**.
- **Appliance levels:** `2, 4, 8, 16, 32, ..., 2048`.
- **Merge Example:**  
  - `2 + 2 → 4`
  - `4 + 4 → 8`
  - `8 + 8 → 16`
  - **Level 2048 items should be removed when clicked.**

### 🏭 Producer Mechanics
- **Producer items generate Appliances** with every click.
- **Produced items appear in the nearest empty cell.**
- If the board is **full**, production is **blocked**.
- **Producers have a capacity**:
  - **Default max capacity:** `10`
  - **Starts with:** `10`
  - **Reduces by `1` with each production**.
  - **If capacity reaches `0`, the producer is replaced** with a **new random producer** on the board.
  - **Capacity increases every `30s` automatically**.

### 🎯 Task System
- **Maximum of 2 active tasks** at a time.
- **Tasks require merging specific appliances** (e.g., "Create Level 8 Appliance").
- **Tasks can be completed by clicking the required appliance**.
- **Completed tasks disappear, and new ones appear**.
- **Task UI shows required appliance levels**.
- **Cells with required appliances are highlighted in green**.
- **Tasks are saved in `PlayerPrefs`**.

### 🔥 Effects & Animations
- **Smooth merging animations** using **DoTween**.
- **Particle effects for merging & inventory interactions**.
- **Highlight effect for task-related items**.

---

## 🎮 Gameplay Summary

### 🔹 Game Flow
1️⃣ Players start on the **8x8 grid board**.  
2️⃣ They **drag and merge items** to create higher levels.  
3️⃣ **Producers generate appliances**, but they **consume capacity**.  
4️⃣ If a producer **runs out of capacity**, it is replaced with a **new producer** in a random location.  
5️⃣ Players complete **tasks by collecting required appliances**.  
6️⃣ Items can be **stored in inventory** for later use.  
7️⃣ **Progress is saved automatically**.

---

## 📂 Grid Structure

The grid state is stored in **`grid_data.json`**, allowing easy modifications.

### 🔹 JSON Grid Example
```json
{
  "grid_width": 8,
  "grid_height": 8,
  "tasks": [
    { "type": "ApplianceA", "level": 4, "capacity": -1 },
    { "type": "ProducerB", "level": 1, "capacity": 10 }
  ]
}
