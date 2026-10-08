# 3dsoulsy — Soulslike Gameplay Systems & Architecture

[![Language](https://img.shields.io/badge/Language-C%23-239120?logo=csharp&logoColor=white)](https://docs.microsoft.com/dotnet/csharp/)
[![Engine](https://img.shields.io/badge/Target%20Engine-Unity%203D-black?logo=unity&logoColor=white)](https://unity.com/)
[![Focus](https://img.shields.io/badge/Format-Pure%20Source%20Code%20(Code%20Review)-orange)](#-codebase-only-notice)
[![Genre](https://img.shields.io/badge/Genre-Action%20RPG%20%7C%20Soulslike-darkred)](#gameplay-systems-breakdown)
[![License](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

A modular gameplay systems architecture and logic codebase for a 3D Soulslike Action-RPG, developed as an academic team project. Features attack combo chains, finite state machine AI, persistent progression, and checkpoint synchronization.

---

## 🌐 Language / Język
- [🇬🇧 English](#-english-version)
- [🇵🇱 Polski](#-wersja-polska)

---

## 💡 Codebase-Only Notice / Uwaga dotycząca zawartości
> **English:** To prioritize clean version control and eliminate repository bloat, heavy 3D assets, textures, audio files, and generated cache folders (`/Library`, `/Temp`) have been stripped. This repository serves as a **clean, focused C# codebase showcase** designed specifically for technical code inspection and code review.
>
> **Polski:** W celu zachowania czystości repozytorium i optymalizacji rozmiaru, usunięto ciężkie pliki binarne, modele 3D, tekstury oraz katalogi tymczasowe silnika Unity (`/Library`, `/Temp`). Repozytorium stanowi **przejrzysty showcase kodu źródłowego w C#**, przygotowany z myślą o sprawnym przeglądzie kodu (Code Review).

---

## 🇬🇧 English Version

### About the Project
**3dsoulsy** is an academic team project implementing core mechanical foundations of the 3D Soulslike genre. The codebase focuses on deterministic combat timing, stamina-dependent actions, hierarchical enemy decision-making, and fail-safe state persistence.

### Gameplay Systems Breakdown

```text
┌────────────────────────────────────────────────────────────────────────┐
│                        GameManager & Core Loop                         │
└──────────────────┬─────────────────────────────────┬───────────────────┘
                   │                                 │
         ┌─────────▼─────────┐             ┌─────────▼─────────┐
         │   Combat & Stats  │             │   Enemy AI FSM    │
         │  • PlayerStats.cs │             │   • EnemyAI.cs    │
         │  • comboScript.cs │             └───────────────────┘
         └─────────┬─────────┘                       │
                   │                                 │
         ┌─────────▼─────────────────────────────────▼─────────┐
         │           World & Persistence Subsystems            │
         │  • CheckpointManager.cs (Respawn & Reset Anchors)   │
         │  • SaveSystem.cs & GameData.cs (JSON / Binary)      │
         └─────────────────────────────────────────────────────┘
