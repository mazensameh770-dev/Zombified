# 🧪 DUNGEON APOTHECARY

A charming Unity-based management game where you craft potions to heal visiting monsters before they lose patience and cause trouble!

---

## 🎮 Game Overview

**DUNGEON APOTHECARY** is a fast-paced, time-management game where players run a potion shop that caters to sick monsters. Your job is simple: identify what each monster needs, gather the right ingredients, brew the perfect potion at your crafting table, and deliver it before they get too impatient. Mess up? Take damage. Survive long enough and watch the difficulty ramp up!

### Core Gameplay Loop

1. **Monsters arrive** at your shop with different ailments
2. **Check their recipe** – each monster needs a specific cure
3. **Gather ingredients** from your stock
4. **Craft the potion** at your crafting table
5. **Deliver it** to the monster before they lose patience
6. **Receive rewards** and prepare for the next patient

---

## 🏗️ Game Architecture

### State Machine Pattern

The game uses a robust **state machine** for monster behavior, making it easy to manage complex AI. Each monster cycles through different emotional states:

- **Waiting State** – Monster arrives and patiently waits for their cure
- **Angry State** – Patience running out! They're getting frustrated
- **Chasing State** – They're so mad they're chasing the player
- **Calming State** – They received the correct cure and are being soothed
- **Exiting State** – Fully healed and leaving the shop

This modular approach means adding new behaviors is as simple as creating a new state class.

### Manager System

The game relies on several core managers (implemented as singletons) to coordinate all the chaos:

#### **GameManager**
The central command center that tracks:
- Game over conditions
- Pause/resume functionality
- Progressive difficulty scaling
- Monster patience and spawn rates that decrease over time

Every 3 monsters served, their patience gets shorter. Every 5 monsters served, they arrive faster. The game gets harder the better you do!

#### **DeliveryManager**
Handles the critical moment – the potion handoff:
- Checks if the delivered cure is correct
- Triggers success or failure events
- Tracks correct deliveries, wrong deliveries, and empty attempts

#### **SpawnManager**
Spawns monsters on a timer that dynamically adjusts based on game progression. Early on, you have breathing room. Later? You'll be juggling multiple patients at once.

#### **ScoreManager & SaveManager**
Track your performance and persist progress between sessions, so players can compete for high scores.

#### **AudioManager**
Manages all sound effects and background music with a unified, event-driven system.

---

## 🧩 Core Systems

### Inventory System

The player has a limited inventory where they can hold items (ingredients and crafted potions). 

**Key Features:**
- Finite slot count forces strategic item management
- Players must strategically drop items if slots are full
- Visual UI shows what items are currently held

### Crafting System

The **CraftingTable** is the heart of production:

1. **Recipe Matching** – The table automatically checks if your inventory contains all required ingredients
2. **Crafting Timer** – Once ingredients are consumed, a timer starts (default 3 seconds)
3. **Item Retrieval** – Once complete, you pick up the finished potion
4. **State Management** – The UI shows clear visual feedback about what's happening

```
Player Interacts → Check Ingredients → Consume Items → Start Timer → Craft Complete → Pick Up Potion
```

### Recipe System

Each monster has a **MonsterRecipe** that defines:
- The specific **cure they need** (ItemSO)
- The **ingredients required** to craft that cure
- Validation logic to check if a delivered item is correct

Recipes are data-driven using ScriptableObjects, making them super easy to configure and balance without touching code.

### Monster Intelligence

Each monster carries its own recipe and tracks:
- How long they've been waiting (patience timer)
- What cure they specifically need
- Whether they'll accept a delivery right now

When patience runs out, they automatically transition to the Angry state and may chase the player, creating moments of panic and comedy.

---

## 📁 Project Structure

```
Scripts/
├── Player/                    # Player movement, interaction, inventory, health
├── Monster/                   # Monster behavior, states, recipes
│   └── States/               # Individual state classes (Waiting, Angry, Chasing, etc.)
├── CraftingTable/            # Crafting logic and visuals
├── Chest/                    # Item storage system
├── Managers/                 # GameManager, SpawnManager, DeliveryManager, etc.
├── UI/                       # All UI screens (Main Menu, Pause, Inventory, Health, etc.)
├── ScriptableObjects/        # Data definitions (Items, Recipes, Sounds)
├── Interfaces/               # IInteractable interface for player interactions
└── (Root Level)              # GameIntro, Loader, EventBridge
```

### Key ScriptableObjects

- **ItemSO** – Base definition for any item (ingredient or cure)
- **CraftedCureRecipeSO** – Recipe linking ingredients to a finished cure
- **RecipeListSO** – Database of all available recipes
- **SoundSO & SoundLibrarySO** – Audio clips organized by type

---

## ⚙️ How Difficulty Scales

The game gets progressively harder as you succeed:

```
Monsters Served:  3      6      9      12     15     ...
Patience:        ↓      ↓      ↓      ↓      ↓     (floors at minPatience)
Spawn Rate:      ✓      ✓      ✓      ✓      ✓     (floors at minSpawnInterval)
```

This creates a natural difficulty curve – players feel the pressure mounting without sudden spikes.

### Customizable Settings

In the GameManager inspector, you can tune:
- **Initial spawn interval** (how often monsters arrive)
- **Initial monster patience** (how long they wait)
- **Patience decrease per 3 serves** (how much harder they get)
- **Spawn rate decrease per 5 serves** (how much faster)
- **Minimum patience & spawn interval** (soft caps to prevent chaos)

---

## 🎯 Player Flow

### Main Menu
Simple entry point – Play, Options, or Quit.

### Game Intro
Brief opening sequence with story/setup. Press interact to skip ahead.

### Gameplay
- Move around the shop
- Interact with chests to gather ingredients
- Approach the crafting table to brew cures
- Deliver to waiting (or increasingly angry) monsters
- Watch your health – wrong deliveries or ignored monsters cause damage

### Game Over
When health reaches zero, the game ends. Your score is displayed based on how many monsters you successfully served.

---

## 🎨 UI System

The UI is event-driven and modular:

- **HealthUI** – Displays current health and damage taken
- **InventoryUI** – Shows carried items and slots
- **InteractionUI** – Prompts when near interactive objects ("Press E to Craft")
- **ScoreUI** – Running tally of monsters served
- **PauseUI** – Menu overlay (pause/resume, options, quit)
- **GameOverUI** – Final score and retry option

Every UI element listens to events from the game systems, so the logic and display are beautifully separated.

---

## 🔧 Technical Highlights

### Design Patterns Used

- **Singleton Pattern** – Managers (GameManager, DeliveryManager, SpawnManager, etc.)
- **State Machine** – Monster behavior and states
- **Observer Pattern** – Events for everything (crafting, delivery, game state changes)
- **Factory/Spawn System** – Dynamic monster spawning
- **ScriptableObject-Based Data** – All game content (items, recipes, sounds) is configuration, not code

### Event System

The game is heavily event-driven. Key events include:
- `OnGameOver`, `OnGamePaused`, `OnGameUnpaused`
- `OnCorrectDelivery`, `OnWrongDelivery`, `OnEmptyDelivery`
- `OnCraftStarted`, `OnCraftFinished`, `OnItemTaken`
- `OnMonsterServed`, `OnHealed`

This decoupling makes the code flexible and easy to extend.

### Save System

Progress is persisted using the **SaveManager**, allowing:
- High score tracking
- Game state restoration between sessions

---

## 📝 License & Credits

Made with ❤️ in Unity. Enjoy the chaos!

---

**Happy potion brewing! 🧪✨**
