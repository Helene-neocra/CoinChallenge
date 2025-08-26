# CoinChallenge - Instructions pour Agent IA

## Architecture du Jeu

**CoinChallenge** est un jeu Unity 3D de collecte de pièces avec timer, utilisant une architecture événementielle pour coordonner la génération procédurale, le gameplay et l'IA des ennemis.

### Composants Principaux

- **FloorGenerator**: Génère un monde procédural et déclenche `OnFloorGenerated` + `OnPlatformGenerated`
- **PlayerController**: Contrôle joueur avec Unity Input System (`PlayerMove.inputactions`)
- **CoinController**: Spawn des pièces sur le terrain généré via événements
- **GameManager**: Gère le game over avec événement `Timer.OnTimeUp`
- **EnnemiGenerator**: Spawn d'ennemis (Slurp/Turtle) avec NavMesh AI

### Patterns Architecturaux

#### 1. Système d'Événements
```csharp
// Pattern utilisé partout pour découpler les composants
public event System.Action<float, float, float, float> OnFloorGenerated;
FloorGenerator.OnFloorGenerated += SpawnCoins; // CoinController s'abonne
```

#### 2. Générateurs Procéduraux
- `FloorGenerator`: Crée terrain avec spacing calculé depuis `RefPointFloor.getDistance()`
- `CoinController`: Place 50 pièces via Raycast depuis position haute vers le sol
- Position joueur calculée automatiquement sur plateforme générée

#### 3. Unity Input System
- Fichier `.inputactions` auto-généré en classe `PlayerMove` 
- Interface `IPlayerActions` pour callbacks Move/Jump
- Pattern Enable/Disable dans OnEnable/OnDisable du PlayerController

#### 4. NavMesh AI pour Ennemis
```csharp
// Pattern standard: Component AgentNavMesh + target Transform
var navMesh = enemy.GetComponent<AgentNavMesh>();
navMesh.target = targetPoint;
navMesh.agent.speed = customSpeed;
```

### Structure des Scènes

1. **Home.unity**: Menu principal
2. **GamePlay.unity**: Scène de jeu principale avec tous les managers
3. **Winner.unity**: Écran de victoire

### Conventions de Code

- **Événements statiques**: `Timer.OnTimeUp` pour game over global
- **FindObjectOfType**: Utilisé pour récupérer singletons (GameManager, Timer, etc.)
- **Raycast pour placement**: Pattern standard pour positionner objets sur terrain
- **Animations via Animator**: States isWalking/isJumping/isFalling sur PlayerController
- **Prefabs organisés**: Assets/Prefabs/ avec variantes (floor1/floor2, arbre1/arbre2/arbre3)

### Workflows de Développement

#### Tests en Play Mode
1. Terrain se génère automatiquement au Start
2. Joueur placé sur PlatformPlayer via événement
3. Timer démarre au premier mouvement joueur
4. Coins apparaissent sur terrain généré

#### Debugging Courant
- Logs détaillés dans PlayerController pour position/timer
- GameManager debug UI activation avec null checks
- FloorGenerator spacing calculé depuis RefPointFloor component

#### Ajout de Nouveaux Éléments
- **Ennemis**: Créer prefab + ajouter à EnnemiGenerator avec vitesse custom
- **Décors**: Ajouter à floorPrefabs[] array pour génération aléatoire
- **Mécaniques**: S'abonner aux événements existants (OnFloorGenerated, OnTimeUp)

### Points d'Intégration

- **Input**: Modifier `PlayerMove.inputactions` puis régénérer classe
- **AI Pathfinding**: NavMesh baking automatique via `NavMeshBaker`
- **UI**: ScoreUI statique, Timer avec TMP_Text, GameManager gère restart
- **Audio**: Assets sons dans BTM_Assets/ et Kevin Iglesias/

### Dépendances Externes

- **Unity Input System**: Package pour contrôles modernes
- **NavMesh Components**: Pour IA des ennemis  
- **TextMeshPro**: Pour UI moderne
- **Assets Store**: Pandazole Nature Pack, RPG Monster DUO pour visuels
- **Cinemachine**: Pour caméras dynamiques
- **Utiliser les composants Unity natifs avant d'implémenter des solutions personnalisées.

Utiliser cette architecture événementielle pour toute nouvelle fonctionnalité. Respecter le pattern singleton via FindObjectOfType pour les managers.
