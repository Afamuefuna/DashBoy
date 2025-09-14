# 🚀 Dash Boy

## 📖 Overview

*A fast-paced action game where the player controls a cylinder that dashes through enemies and collects power-ups.*  
Enemies spawn in waves that grow stronger over time, making survival increasingly difficult.

---

## 🎮 Core Mechanics

* **Movement** → `WASD` / Arrow Keys  
* **Dash** → `Spacebar` (dash in current movement direction)  
* **Dash + Direction** → Arrow + `Spacebar` (dash toward a chosen direction)  
* **Power-Ups**:
  * 🌀 *Double Dash*: Grants an extra dash per cooldown  
  * ⚡ *Speed Boost*: Temporarily increases movement speed  
* **Enemies**: Destroyed when dashed into; become stronger in later waves  

---

## 🏗️ Architecture

This project was built with **SOLID principles** and clean, modular design.  
It also makes use of the **Unity New Input System** for handling all player controls.

### 🔹 Scriptable Objects
* Store **Enemy Data**, **PowerUp Data**, and **Projectile Data**  
* Makes balancing and tweaking values possible without editing scripts  

### 🔹 Design Patterns
* **Visitor Pattern** → Handles applying power-up logic  
* **State Machine Pattern** → Governs enemy AI behaviors  
* **Observer Pattern (Game Events)** → Decouples systems (e.g., score, wave updates)  
* **Inheritance** →  
  * `EnemyBase` (abstract parent)  
    * `ChaserEnemy` and `ShooterEnemy` extend behavior  
  * `PowerUp` (abstract parent)  
    * `DoubleDashPowerUp` and `SpeedBoostPowerUp` extend behavior  

### 🔹 Core Systems
* `PlayerController` → Movement, dash, collision handling  
* `EnemyBase` + State Machine → AI framework for enemies  
* `WaveSystem` → Controls wave spawning and difficulty scaling  
* `PowerUpSystem` → Manages power-up effects, stacking, and extensions  
* `AudioManager` → Centralized sound and feedback system  

---

## 🎮 Controls

| Action               | Key(s)                 |
| -------------------- | ---------------------- |
| **Move**             | `WASD` / Arrow Keys    |
| **Dash**             | `Spacebar`             |
| **Dash + Direction** | Arrow Key + `Spacebar` |

---

## 📚 References

* [Unity Documentation](https://docs.unity3d.com/) (Scripting, ScriptableObjects, State Machines, **New Input System**)  
* Design Pattern references for **Visitor** and **State Machine**  
* General **SOLID principle** guidelines for maintainable architecture  

---
