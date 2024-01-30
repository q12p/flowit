# Flowit

## Comment jouer

Il faut remplir toutes les cases "normales" avec leur couleur de remplissage avec les boutons spéciaux comme les cercles, flèches et bombes.

## Fonctionnement

Le jeu c'est un tableu en deux diménsions qui a des boutons (class). Chaque bouton a un couleur, un type et un remplissage. (Une diréction aussi pour les flèches).

1. Le fonctionnement est simple, on va pour chaque niveau remplir avec le fichier json les données de chaque bouton dans notre tableau level.

2. Après, on va en boucle checker si toutes les boutons normales, ont le même couleur de remplissage que leur valeur de couleur.

3. Chaque fois que le jouer touché l'écran, on va calculer quel bouton était touché et déclancher ses fonctions spéciaux s'il en a.

4. Si le derniere niveau est completé, on sort du jeu.

## Executer

Si le fichier executable est lance sur le dossier "executable", il devrait foncitonner. Je n'avais pas le temps pour faire une installation prope.

Il faut s'assurer de compiler avec NewtonSoft.Json dans la version 13.0.3

## À faire

- [ ] Améliorer le dessin des flèches tournantes

- [ ] Optimiser les variables dynamiques dans un setUp() pour éviter des calculs innécessaires

- [ ] Variables dynamiques


## Notes



## Si j'ai le temps

- [ ] Sélection de niveau (menu). Pas de temps
- [ ] Animation menu initial. Pas de temps
