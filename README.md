# 3dsoulsy - Soulslike Game Logic & Systems

---

## 🇵🇱 Wersja Polska

### O projekcie
Projekt akademicki realizowany w zespole w ramach przedmiotu uniwersyteckiego. Jest to trójwymiarowa gra akcji z gatunku RPG inspirowana mechanikami typu Soulslike.

Repozytorium zawiera kompletną logikę rozgrywki oraz architekturę systemów napisaną w języku C#. Kod odpowiada m.in. za:
* System walki, statystyki postaci oraz rozwijanie poziomu gracza (`PlayerStats.cs`, `comboScript.cs`).
* Sztuczną inteligencję przeciwników (`EnemyAI.cs`).
* System zapisu i odczytu stanu gry (`SaveSystem.cs`, `GameData.cs`).
* Zarządzanie punktami kontrolnymi (`CheckpointManager.cs`) oraz globalnym stanem gry (`GameManager.cs`).

### Dlaczego w repozytorium znajdują się tylko pliki kodu?
Ze względu na ograniczenia miejsca na dysku oraz potrzebę zachowania przejrzystości, z repozytorium zostały celowo usunięte wszystkie ciężkie pliki binarne silnika Unity, tekstury, modele 3D oraz foldery tymczasowe (`Library`, `Temp`). 

Projekt został odchudzony tak, aby zawierał wyłącznie czysty kod źródłowy (100% skrypty C#) przeznaczony do szybkiego przeglądu (Code Review).

---

## 🇬🇧 English Version

### About the Project
An academic group project developed for a university course. It is a 3D Action-RPG game heavily inspired by Soulslike mechanics.

This repository features the complete gameplay logic and systems architecture written in C#. The codebase manages:
* Combat systems, player statistics, and leveling progression (`PlayerStats.cs`, `comboScript.cs`).
* Enemy Artificial Intelligence (`EnemyAI.cs`).
* Save and load state persistence pipelines (`SaveSystem.cs`, `GameData.cs`).
* Checkpoint synchronization (`CheckpointManager.cs`) and global game loop states (`GameManager.cs`).

### Why does this repository contain only code files?
To optimize disk storage and maintain codebase clarity, all heavy binary files, textures, 3D models, and engine-specific temporary folders (`Library`, `Temp`) have been intentionally removed from the repository.

The project has been streamlined to showcase strictly the native source code (100% C# scripts) for seamless Code Review.
