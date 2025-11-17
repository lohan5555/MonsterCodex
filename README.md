Ce projet et un jeu mobile développé avec le moteur de jeu Unity, dans le cadre d'un cours de 1ère année de master d'informatique par Lohan Hébert et Lenny Loy. 
Ce jeu est un "jeu sérieux", c'est-à-dire qu'il est à porter pédagogique. Tout au long de l'aventure, le joueur pourra apprendre des informations sur les différents monstres de notre univers.

# I - Installation

Pour lancer le jeu, cloner le dépôt git en haut à droite et exécuter le avec Unity.

Voici le lien de la vidéo de démo : [vidéo](https://www.youtube.com/watch?v=dWatA9x3PPQ) .

/!\ la vitesse de déplacement de la caméra peut varier d’un téléphone à un autre. Pour régler ça: Asset → Scene → LoadingScene → Player → UI_TouchScreenInput → TouchScrennInput.cs  → look magnitude multiplayer (par défaut 50, essayer à 4)

# II - Choix et fonctionnalités

## Liste des fonctionnalités:

- Un menu principal au lancement du jeu, permettant de le lancer ou de quitter l’application.
- Un écran de chargement avant que les données du jeu ne soient chargées.
- Une interface permettant de déplacer un avatar dans un environnement 3D avec un joystick, de déplacer une caméra autour de cet avatar et des boutons permettant de sauter, de courir, de parler à un PNJ (quand on est suffisamment proche de celui-ci), de mettre le jeu en pause et d’attaquer.
- Un menu pause avec plusieurs options: retour au menu principal (réinitialisation de la progression), reprendre la partie et ouvrir un menu Grimoire.
- Combattre des monstres: infliger des dégâts, subir des dégâts, tuer un monstre, récupérer un objet lâché par un monstre, se faire tuer et ressusciter (réinitialisation du niveau mais pas des connaissances acquises).
- Passer au niveau supérieur si les conditions sont remplies (collecter des items spécifiques).
- Parler à un PNJ avec plusieurs dialogues possibles en fonction de l’état de l’inventaire.
- Un menu Grimoire affichant la liste des informations que le joueurs à apprise en combattant ou en interagissant avec un PNJ.

# III - Architecture

- La scène LoadingScene sert à instancier tous les GameObject avec la propriété DontDestroyOnLoad ainsi que de récupérer les données du serveur (et de permettre au joueur de retenter en cas d'échec).
- Les données d’apprentissage et de dialogues sont stockées sous la forme de fichiers JSON hébergés avec le service GitHub Pages: [github pages](https://github.com/BillyBoy16/unity-dialogues) . 
- Un empty GameObject gameManager sert à effectuer les requêtes au serveur. La récupération des données ne se fait qu’une seule fois au lancement.
- Un empty GameObject spawnPoint est présent sur chaque niveau pour indiquer au Player son lieu d’apparition.
- Utilisation de LSF sur git pour les fichiers volumineux.
- Nom du package: com.codeminds.monsterCodex


# IV - Difficultés rencontrées

Impossible de fusionner proprement des branches avec Git, car cela corrompait nos fichiers. On a commencé le projet en faisant une branche par fonctionnalité, mais on a fini par tout faire sur la branche « develop » (soit en programmation en binôme, soit en travaillant par intervalles).

# V - Retour personnel

Une expérience amusante et instructive. Nous avons su aisément nous répartir les différentes fonctionnalités à développer selon nos envies, mais le délai était trop court car nous aurions aimé aller plus loin. Cependant, nous sommes satisfaits du produit rendu et du projet en général.

# VI - Image du jeu

Menu principal:
<img width="2400" height="1080" alt="image" src="https://github.com/user-attachments/assets/d7d0cf08-8847-4c21-969a-d26552bad4ab" />

Écran de chargement en cas d’erreur:
<img width="2400" height="1080" alt="image" src="https://github.com/user-attachments/assets/7d391558-e680-44eb-8fcc-3f574983d316" />

Niveau 1:
<img width="2400" height="1080" alt="image" src="https://github.com/user-attachments/assets/5c85bf19-5124-4e9a-b900-5b9aa31e0473" />

Niveau 2:
<img width="2400" height="1080" alt="image" src="https://github.com/user-attachments/assets/9134495a-fd6b-4c7e-830f-e065e86faec9" />

Niveau 3:
<img width="2400" height="1080" alt="image" src="https://github.com/user-attachments/assets/8927aae5-cf0d-457f-97af-646933becce9" />

Dialogue avec un PNJ:
<img width="2400" height="1080" alt="image" src="https://github.com/user-attachments/assets/05aec022-d1ec-44ee-9c67-5099bc0624f3" />

Menu pause:
<img width="2400" height="1080" alt="image" src="https://github.com/user-attachments/assets/94c2602d-b86d-404b-b45a-36fc362da7d6" />

Menu Grimoire avec deux savoirs acquies:
<img width="2400" height="1080" alt="image" src="https://github.com/user-attachments/assets/530347f6-7216-4b1e-b1e6-61312d2de4d7" />

Combat avec un monstre:
<img width="2400" height="1080" alt="image" src="https://github.com/user-attachments/assets/fb2b30d8-b8f4-4d3f-a03d-274249da04cd" />

Objet lâché par un monstre à sa mort:
<img width="2400" height="1080" alt="image" src="https://github.com/user-attachments/assets/2ecf9aad-0f83-46f3-9943-4fca5ca83a6d" />

Boss de fin invoquant une tornade de feu:
<img width="2400" height="1080" alt="image" src="https://github.com/user-attachments/assets/1286f3a3-1039-4c73-a007-5c630856e805" />

# VII - Liste des assets:

Environnement niveau 1: https://assetstore.unity.com/packages/3d/environments/lowpoly-environment-nature-free-medieval-fantasy-series-187052 

Environnement niveau 2: https://assetstore.unity.com/packages/3d/environments/landscapes/lowpoly-environment-pack-99479 

Environnement niveau 3: https://assetstore.unity.com/packages/3d/environments/low-poly-tropical-island-lite-242437 

Modèle du PNJ: https://assetstore.unity.com/packages/3d/characters/humanoids/lowpoly-magician-rio-288942 

Objets lâcher par les monstre: https://assetstore.unity.com/packages/3d/props/simple-gems-and-items-ultimate-animated-customizable-pack-73764 

Particule de feu: https://assetstore.unity.com/packages/vfx/particles/spells/fire-attack-vfx-the-ultimate-pack-306217 

Particule de fumée: https://assetstore.unity.com/packages/vfx/particles/fire-explosions/free-stylized-smoke-effects-pack-226406 

Modèle de la stèle pour changer de niveau: https://assetstore.unity.com/packages/3d/props/poly-halloween-pack-236625 

Design des boutons des menus: https://assetstore.unity.com/packages/2d/gui/icons/2d-rpg-button-7-278861 

Avatar du joueur: https://assetstore.unity.com/packages/3d/characters/humanoids/rpg-tiny-hero-duo-pbr-polyart-225148

Modèles des monstres: https://assetstore.unity.com/packages/3d/characters/creatures/rpg-monster-buddy-pbr-polyart-253961 

Input controller: https://assetstore.unity.com/packages/essentials/starter-assets-character-controllers-urp-267961#content 

Barre de vie: https://assetstore.unity.com/packages/tools/gui/tinyhealthsystem-v1-0-181303 

Musique : https://lasonotheque.org
