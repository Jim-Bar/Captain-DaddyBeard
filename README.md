# Captain-DaddyBeard
A multimodal game playable on PC and Android simultaneously !


Configuration requise :
=======================

- Un PC sous Windows (7 conseillé) avec wifi 
- Les droits administrateurs sur ce PC
- Une tablette (Nexus 7 conseillée)

A propos du Git
===============

Il y a deux branches différentes sur le Git. La branche soutenance est l'état du projet tel qu'il était au jour de la soutenance le 11/02/15. La branche master possède plusieurs améliorations et corrections de bugs. On notera en particulier la mise en place d'une interface responsive, et le recul de la caméra par rapport à la scène, ce qui rend le jeu beaucoup plus jouable. Il est conseillé de préférer la branche master à la branche soutenance.

Si vous utilisez la branche master et clonez le projet via Git, pensez à activer la création du résea wifi automatique. Pour ce faire, dans Unity, cherchez le prefab "Game Manager Server" situé dans "Assets/Scenes/Home Scene/Windows/Prefabs" et cochez la case "Create Wifi hotspot" du script "ServiceConnector.cs". Si vous utilisez directement le build, il n'y a rien à faire.

Où trouver les builds ?
=======================

Les Builds sont présents sur Google Drive à cette adresse : https://drive.google.com/folderview?id=0B21ptyZsvR0JZzRoWGR5NmdaN1E&usp=sharing

Comment l’installer ?
=====================

Sur le PC
---------

Copier le dossier contenant le jeu n’importe où sur l’ordinateur.

Sur la tablette (testé avec une Nexus 7)
----------------------------------------

Installer le package du jeu (.apk) sur la tablette. Il existe plusieurs solutions pour cela, se référer à Google pour le faire.
Alternativement, le package peut être automatiquement installé via USB en faisant un build Android du jeu depuis le projet Unity. La tablette doit avoir le mode développeur activé, et l’Android SDK doit être installé sur l’ordinateur.

Comment le lancer ?
===================

Considération de Wifi
---------------------

La connexion entre le PC et la tablette se fait par Wifi. Deux solutions sont possibles :
connecter le PC et la tablette au même réseau wifi manuellement puis lancer le jeu
laisser le PC créer le réseau wifi, et la tablette se connecter au réseau

Dans tous les cas, l’application Windows doit être lancée avant celle sur tablette, et **le réseau wifi doit être créé avant de lancer l’application tablette**. Dans la suite, nous décrivons la deuxième solution.

Sur le PC
---------

Démarrer l’application en **mode administrateur** (autorise la création du réseau wifi). Ensuite, attendre que le réseau soit créé. Le réseau s’appelle “Daddy_Beard_Server”. Vous pouvez consulter son état parmi les réseaux auxquels vous êtes connectés via l’interface Windows. Le réseau est prêt quand il passe de l’état “No network access” à l’état “No Internet access”. Ceci peut prendre jusqu’à deux minutes suivant les ordinateurs.

Sur la tablette
---------------

Attendre que le réseau soit prêt. Quand il est prêt, lancez l’application. Le jeu devrait se lancer et se connecter automatiquement. Quand la connexion a réussi, le dessin de la tablette se colore. N’oubliez pas d’activer le wifi de la tablette !

Comment jouer ?
===============

Une fois les des applications lancées et connectées, la tablette demande une calibration. Suivez les instructions de la tablette.
Une fois ceci fait, un viseur apparaît sur l’écran de l’ordinateur. Visez “Play”, et appuyez sur le bouton “Feu !” de la tablette. Vous arrivez sur le menu du choix du niveau. Sélectionnez en un en appuyant dessus, visez “Valider” et tirez. La sélection de l’arme est identique, à l’exception (certes illogique) qu’il faut glisser l’arme vers le haut de la tablette pour la sélectionner.
Le jeu commence ! Un message vous dit que faire avec la tablette.
En phase de déplacement, tenez la tablette à plat et penchez la à droite et à gauche pour faire tourner le monde et déplacer Captain DaddyBeard. Évitez de tomber, ramassez les bonus et essayez d’arriver à la prochaine patate.
En phase de tir, tenez la tablette droite de manière à viser l’écran, et appuyez sur “Feu !” pour tirer. Vous pouvez activer le scan (consomme de l’énergie !) pour trouver des bonus cachés dans les nuages...
En cas de problème, vous pouvez recalibrer la tablette depuis le menu pause ou depuis les menus du jeu (bouton en haut à gauche).

*Dépôt Git : https://github.com/Zer0Zer0Huit/Captain-DaddyBeard*