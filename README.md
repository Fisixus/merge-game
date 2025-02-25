# 🧩 Merge Game (Travel Town-Inspired)

A **merge-based puzzle game** built in Unity using C#. Players merge items to create higher-level objects and complete tasks. The game features an inventory system, level-based progression, and various mergeable objects.

## 📖 About the Project

This project was developed as a **case study** and is not intended for commercial use. It replicates the core mechanics of **Travel Town-style** merging games while implementing **clean code architecture** and efficient **design patterns**.

### 🛠️ Built With
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

The project follows **Model-View-Presenter (MVP)** architecture to ensure:
✅ **Separation of Concerns** – Clear distinction between game logic and UI.  
✅ **Scalability** – Easy to expand with new mechanics.  
✅ **Maintainability** – Code remains clean and modular.  

### 🔹 Key Components
#### **1️⃣ Models**
- Responsible for **storing and managing game data**.
- Encapsulate the **state and rules** of the game.
- **Examples:**
  - **Grid data** for grid.
  - **Player inventory & merge logic**.
  - **Task management system**.

#### **2️⃣ Views**
- Handle **visual feedback** and **user interactions**.
- Render the **UI and scene elements** but do not contain game logic.
- **Examples:**
  - **Game grid display**.
  - **Inventory UI**.
  - **Task UI**.

#### **3️⃣ Presenters**
- **Bridge between Models & Views**.
- Process **user input**, update models, and synchronize the UI.
- **Examples:**
  - **InventoryPresenter** – Manages inventory system.
  - **MergePresenter** – Handles merging logic.
  - **TaskPresenter** – Updates tasks and goal tracking.

#### **4️⃣ Handlers**
- **Abstract business logic** from Presenters.
- Break down **specific game mechanics** into **manageable components**.
- **Examples:**
  - **GridPawnFactoryHandler** – Handles factories.
  - **EffectHandler** – Handles effect management.

#### **5️⃣ Factories**
- **Efficient object creation and pooling**.
- Ensure **optimized performance** by reusing objects.
- **Examples:**
  - **ApplianceFactory** – Creates and recycles appliances.
  - **ProducerFactory** – Creates and recycles producers.

---

## 🛠️ Features

### 🗺️ Dynamic Grid System
- Players **progress through tasks**, each with unique goals.
- **Merge items** to create higher-level objects.
- **Grid structure is JSON-based**, making it **easy to modify**.

### 📦 Inventory System
- Players can **store** mergeable items in their **inventory**.
- Items can be **dragged and dropped back** to the grid.
- **Inventory saves & loads** using JSON serialization.

### 🔄 Merge Mechanics
- **Merge 2 identical items** to create a **higher-level item**.
- **Different item types** (Appliances, Producers, Boosters).
- Producers have a **capacity system** to limit item generation.

### 🎯 Task System
- Players receive **tasks with specific goals**.
- **Task progress saves and reloads automatically with playerprefs**.
- **New tasks** can be created with scriptable objects.

---

## 🎮 Gameplay Summary

### 🔹 Merge Mechanics
- **Players tap & drag items to merge**.
- **If an item reaches max level**, it **can be used for tasks**.

### 🔹 Task System
- Each level **has a set of goals**.
- **Goals track item merges** (e.g., "Create 3 Level 4 Appliances").

---

## 📂 Grid Structure
Grid is defined by **grid_data.json**, making it easy to edit or create new levels.

### 🔹 JSON Grid Example
```json
{
  "grid_width": 8,
  "grid_height": 8,
  "tasks": [
    { "type": "ApplianceA", "level": 3, "capacity": -1 },
    { "target": "ProducerB", "level": 1, "capacity": 10 }
  ]
}

## 📂 How to Use

Clone the repository:
   ```bash
   git clone https://github.com/fisixus/paxie-merge-game.git
