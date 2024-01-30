/*******************************************************************************************
*
*   Flowit! - Izquierdo, Pedro 2023
*   
*   Projet sur Raylib pour atelier informatique basé sur le jeu : Flowit! https://github.com/Flowit-Game/Flowit
*   
*   !!!
*   C'est jeu n'est pas fait ni pour être commercialisé ni pour remplacer le devéloppement du jeu original.
*   Il est uniquement crée avec le but d'apprendre.
*   !!!
*   
********************************************************************************************/

using System.Numerics;
using System.Text.Json;
using Raylib_cs;
using Flowit;
using static Raylib_cs.Raylib;
using System.Text.Json.Nodes;
using Newtonsoft.Json;

namespace Flowit
{
    public class Flowit
    {
        // Couleurs personnalisés
        public static Color MyGRAY = new Color( 125, 106, 108, 255);
        public static Color BACKGROUND = new Color( 229, 229, 229, 255);
        public static Color MyORANGE = new Color( 242, 123, 20, 255);

        public static int Main()
        {
            // Initialization

            // Dimension de la fenêtre    // La dimension de la fenêtre est calculé selon la largeur y/1.583333333  
            // Par défaut 950 si l'écran est full-HD (1920x1080)
            int screenHeight = 950;   // Y
            int screenWidth = Convert.ToInt32(screenHeight/1.583333333);   // X

            // Initialisation avec les dimensions et titre de l'écran
            InitWindow(screenWidth, screenHeight, "Flowit! - Izquierdo, Pedro");

            // Icône d'application
            SetWindowIcon(LoadImage(@"img\icon.png"));

            // Variable qui nous permet de savoir à quel point on est dans le jeu
            // -1 : menu    0 : sélection de niveau     1 : niveau 1      2 : niveau 2
            // On va utiliser 0 : menu parce qu'on n'a pas un sélecteur des niveaux
            int game_state = 0;

            // Quantité de fois que le joueur a touché une case
            int coups = 0;

            // Meilleur score
            int best = 0;

            // Variable pour sortir du jeu    TRUE = Le jeu se ferme
            bool quitter = false;

            // FPS - On va utiliser 30 parce que le jeu n'a pas besoin de plus en tant que puzzle.
            SetTargetFPS(30);

            // On laisse la touche "NULL" pour sortir. De cette manière, on ne peut pas sortir en touchant ESC
            SetExitKey(KeyboardKey.KEY_NULL);

            // On obtient la distance de notre text avec titre_distance pour centré le text dans l'écran
            // La taille de la police est calculé par rapport à la largeur de l'écran. Avec un écran de 600x950, on aura 100 comme taille
            int titre_distance_font_size = Convert.ToInt32(screenHeight/9.5);
            int titre_distance = Raylib.MeasureText("FLOW IT !", titre_distance_font_size);

            // Taille de la police du texte de nos boutons du menu calculé par rapport à l'écran
            int boutons_text_font_size = Convert.ToInt32(screenHeight/19);
            // La largeur et longeur de nos boutons calculées par rapport à l'écran
            int boutons_largeur = Convert.ToInt32(screenHeight/12.66666667);
            int boutons_longeur = Convert.ToInt32(screenHeight/2.375);

            // On mésure les textes du menu pour les centrer
            int start_distance = Raylib.MeasureText("START", boutons_text_font_size);
            int settings_distance = Raylib.MeasureText("SETTINGS", boutons_text_font_size);
            int exit_distance = Raylib.MeasureText("EXIT", boutons_text_font_size);

            // Souris
            Vector2 mousePoint = new Vector2(0.0f, 0.0f);

            // Variables pour les boutons du menu principale. Elles sont true quand les boutons est touché
            bool click_start = false;
            bool click_exit = false;

            // Tableau pour les niveaux. On va initialiser le niveau 1
            Bouton[,] level = creerTableauNiveau(1);

            //-----------------------------------------------------

            // Loop main
            while (!WindowShouldClose() && !quitter)
            {
                // Menu
                if ( game_state == 0)
                {
                  // On initialise la souris
                  mousePoint = GetMousePosition();

                  BeginDrawing();
                  // On utilise BACKGROUND pour le couleur de fond
                  ClearBackground(BACKGROUND);

                  // Titre centré avec la distance de la fenêtre moins la distance du texte entre 2 pour mettre au milieu
                  // Le marge pour centrer est divisé entre 2 pour avoir le même espace de deux côtes
                  // La distance Y est calculé selon une division de 19 fois la largeur de la fenêtre
                  DrawText("FLOW IT !", (screenWidth - titre_distance)/2, Convert.ToInt32(screenHeight/19), titre_distance_font_size, Color.ORANGE);


                  // CALCULES DE L'INTERFACE : Les positions X, Y et la police sont calculés par rapport à l'écran, c'est pour ça qu'on a beacoup des nombres qui semblent aléatoires

                  // Bouton START
                  // On commence à X = 0 parce qu'il doit être à gauche
                  // La position Y est calculé selon la division 3.544... Ceci c'est l'espace par défaut de l'interface
                  // On utilise la même taille de police pour les boutons
                  // Les textes sont alignés à droite avec un petit marge qui équivaut à la distance du bouton - sa distance / 10 Par défaut: 400 - 40 = 360
                  Raylib.DrawRectangle(0, Convert.ToInt32(screenHeight/3.544776119), Convert.ToInt32(screenWidth/1.5), boutons_largeur, MyORANGE);
                  DrawTriangle(
                    new Vector2(boutons_longeur, Convert.ToInt32(screenHeight/3.544776119)),
                    new Vector2(boutons_longeur, Convert.ToInt32(screenHeight/2.769679300)),
                    new Vector2(Convert.ToInt32(screenHeight/2.111111111), Convert.ToInt32(screenHeight/3.114754098)), MyORANGE);
                  DrawText("START", (boutons_longeur - boutons_longeur/10) - start_distance, Convert.ToInt32(screenHeight/3.368794326), boutons_text_font_size, Color.WHITE);
                  // Ceci c'est la hitbox que notre mouse va "toucher"
                  Rectangle btnStart = new Rectangle(0, Convert.ToInt32(screenHeight/3.544776119), boutons_longeur, boutons_largeur);
                              
                  // Bouton SETTINGS
                  Raylib.DrawRectangle(0, Convert.ToInt32(screenHeight/2.060737527), boutons_longeur, boutons_largeur, MyORANGE);
                  DrawTriangle(
                    new Vector2(boutons_longeur, Convert.ToInt32(screenHeight/2.056277056)),
                    new Vector2(boutons_longeur, Convert.ToInt32(screenHeight/1.775700935)),
                    new Vector2(Convert.ToInt32(screenHeight/2.111111111), Convert.ToInt32(screenHeight/1.907630522)), MyORANGE);
                  DrawText("SETTINGS", (boutons_longeur - boutons_longeur/10) - settings_distance, Convert.ToInt32(screenHeight/2), boutons_text_font_size, Color.WHITE);

                  // Bouton EXIT
                  Raylib.DrawRectangle(0, Convert.ToInt32(screenHeight/1.452599388), boutons_longeur, boutons_largeur, MyORANGE);
                  DrawTriangle(
                    new Vector2(boutons_longeur, Convert.ToInt32(screenHeight/1.452599388)),
                    new Vector2(boutons_longeur, Convert.ToInt32(screenHeight/1.303155007)),
                    new Vector2(Convert.ToInt32(screenHeight/2.111111111), Convert.ToInt32(screenHeight/1.372832370)), MyORANGE);
                    DrawText("EXIT", (boutons_longeur - boutons_longeur/10) - exit_distance, Convert.ToInt32(screenHeight/1.422155689), boutons_text_font_size, Color.WHITE);
                  Rectangle btnExit = new Rectangle(0, Convert.ToInt32(screenHeight/1.452599388), boutons_longeur, Convert.ToInt32(screenHeight/12.66666667));

                  // Bouton START touché
                  if (IsRectangleTouched(ref mousePoint, btnStart, ref click_start))
                  {
                      // game_state = 1 pour commencer directement le niveau 1, vu qu'on n'a pas de sélecteur
                      game_state = 1;
                      // On réinitialise les coups
                      coups = 0;
                  }

                  // Bouton EXIT touché
                  if (IsRectangleTouched(ref mousePoint, btnExit, ref click_exit))
                      // On quitte le jeu
                      quitter = true;
                  }

                  // Niveaux
                  // Entre 0 et 8 parce qu'on a 8 niveaux
                  else if (game_state > 0 && game_state < 8)
                  {
                      // On initialise le niveau avec le score et coups à 0
                      bestScoreNiveau(ref game_state, ref best);
                      InitialisationNiveau(ref level, ref game_state, ref quitter, ref coups, ref best);
                  }

                  EndDrawing();
                }

            // De-Initialisation
            CloseWindow();
            return 0;
        }

        // Set up TO-DO
        static void setUp()
        {
        }            

        // Initialisation de chaque niveau. Ici on appelle les variables qui initialise les niveaux
        static private void InitialisationNiveau(ref Bouton[,] level, ref int game_state, ref bool quitter, ref int coups, ref int best)
        {
            // On regarde l'événement clique
            JoueurFaitClique(ref level, ref game_state, ref coups, ref best);

            BeginDrawing();
            ClearBackground(BACKGROUND);


            // Génération de tableau (GRAPHIQUEMENT parlant)
            DessinTableauNiveau(ref level, ref coups, ref best);

            // Le joueur a gagné
            if (win(ref level))
                winMessage(ref game_state, ref quitter, ref coups, ref level);
                // On affiche alor le message gagnant, on génére le prochain niveau et on passe au suivante ou si dans le dernière niveau, on ferme le jeu
        }
        // Fonction pour voir si un bouton était touché
        static private bool IsRectangleTouched(ref Vector2 mousePoint, Rectangle btn, ref bool click)
        {
          // Le but c'est de activer la fonction du bouton que s'il était touché et rélaché dans sa position
            if (CheckCollisionPointRec(GetMousePosition(), btn))
            {
                if (IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT) && click == false)
                {
                    click = true;
                }
                if (IsMouseButtonReleased(MouseButton.MOUSE_LEFT_BUTTON) && click == true)
                {
                    click = false;
                    return true;
                }
            }
            else if (IsMouseButtonReleased(MouseButton.MOUSE_LEFT_BUTTON))
            {
                click = false;
            }
            return false;
        }
        
        // Fonction recursive qui permet à un cercle de remplir tous les cases
        static void remplissage4( ref Bouton[,] level, int Y, int X, Couleurs cible, Couleurs remplir, ref int coups, ref int best)
        {
          // Le fonctionement est simple, on va remplir la case où on se retrouve et après, en haut, en bas, à droite et à gauche. Avec la recursivité on s'assure de faire toutes les cases possibles
            try
            {
                if (level[Y, X].getRemplisageBouton() == cible && level[Y, X].type != 0 && level[Y, X].type != 2 && level[Y, X].type != 3 && level[Y, X].type != 5) // cible == vide
                {
                  // On regarde si le type est 1, c'est-à-dire, un cercle
                    if (level[Y, X].type == 1 && level[Y, X].couleur != remplir)
                        Console.Write("");
                    else
                    {
                        level[Y, X].remplisage = remplir;
                        // Pour montrer dinamiquement le remplisage au joueur
                        BeginDrawing();
                        DessinTableauNiveau(ref level, ref coups, ref best);
                        EndDrawing();
                    }
                    // -1 et +1 à X et Y pour regarder les cases adjacentes
                    remplissage4(ref level, Y - 1, X, cible, remplir, ref coups, ref best); // HAUT
                    remplissage4(ref level, Y + 1, X, cible, remplir, ref coups, ref best); // BAS
                    remplissage4(ref level, Y, X + 1, cible, remplir, ref coups, ref best); // DROITE
                    remplissage4(ref level, Y, X - 1, cible, remplir, ref coups, ref best); // GAUCHE
                }

            } catch { }
        }

        // Fonction qui regarde si un cercle est entoure par au moins une seule case vide
        static private bool existeCasesRemplir(ref Bouton[,] niveau1, int Y, int X)
        {
            try
            {
              // Le type 4 équivaut à une case normale (remplissable)
              // -1 et +1 pour regarder les cases adjacentes
              // true si une case vide est à côté. false si ce n'est pas le cas
              // Couleurs.VIDE parce qu'une case NULL n'a même pas de couleur
                if (niveau1[Y - 1, X].type == 4 && niveau1[Y - 1, X].getRemplisageBouton() == Couleurs.VIDE)
                    return true;
            } catch { }
            try
            {
                if (niveau1[Y, X + 1].type == 4 && niveau1[Y, X + 1].getRemplisageBouton() == Couleurs.VIDE)
                    return true;
            } catch { }
            try
            {
                if (niveau1[Y + 1, X].type == 4 && niveau1[Y + 1, X].getRemplisageBouton() == Couleurs.VIDE)
                    return true;
            } catch { }
            try
            {
                if (niveau1[Y, X - 1].type == 4 && niveau1[Y, X - 1].getRemplisageBouton() == Couleurs.VIDE)
                    return true;
            } catch { }
            return false;
        }

        // Fonction qui retourne true si on a gagné (tous les cases son du même couleur)
        static private bool win(ref Bouton[,] niveau1)
        {
          // On regarde toutes les cases et on va comparer leurs couleur de remplissage avec le couleur normal
            for (int y = 0; y < niveau1.GetLength(0); y++)
                for (int x = 0; x < niveau1.GetLength(1); x++)
                {
                  // Type 4 pour les cases normales
                    if (niveau1[y, x].type == 4 && niveau1[y, x].couleur != niveau1[y, x].remplisage)
                      return false;
                }

            // On a gagné
            return true;
        }

        // Affichage du message gagnant
        static private void winMessage(ref int game_state, ref bool quitter, ref int coups, ref Bouton[,] level)
        {
          // On affiche dans la position 0 jusqu'au la fin de l'écran. 316 parce que je n'ai pas eu le temps de rendre dynamique la variable de dessin d'interface
            DrawRectangle(0, 316, 600, 316, Color.GRAY);

            // Centrage du texte. 470 pour être dédans le rectangle dessiné avant. 50 de police, la même que les boutons du menu ppal. Pas de temps pour rendre dynamique
            DrawText("LEVEL COMPLETE", (GetScreenWidth() - 470)/2, 430, 50, Color.WHITE);

            EndDrawing();

            // On attend trois seconds et on passe au niveau suivante (sauf si on est dans le niveau 5)
            // 7 parce qu'on a 7 niveaux
            if (game_state < 7)
            {
              // 3 secondes
                Thread.Sleep(3000);
                // Prochain niveau
                game_state++;

                modifierTableauNiveau(game_state, ref level);
                // Réinitialisation des coups
                coups = 0;
            }
            // Si on est dans le derniere niveau, on quitte le jeu
            else
            {
                // 3 secondes
                Thread.Sleep(3000);
                quitter = true;
            }
        }

        // On dessine là les deux triangles qui nous laisse aller en arrière ou avancer dans le jeu
        static private void menuSuperieur()
        {
            // Décoration
            // 0, 0 pour commencer en haut à gauche. 120, Pas le temps de rendre dynamique l'interface
            DrawRectangle(0, 0, GetScreenWidth(), 120, Color.LIGHTGRAY);

            // Beacoup des variables de dessin d'interface dont j'en n'ai pas eu le temps de rendre dynamique

            // Bouton arrière
            DrawTriangle(
                new Vector2(30, 70),
                new Vector2(65, 105),
                new Vector2(65, 45), MyGRAY);

            // Bouton suivante
            DrawTriangle(
                new Vector2(535, 105),
                new Vector2(570, 70),
                new Vector2(535, 45), MyGRAY);

            // Bouton réinitialiser
            // Petit bouton en haut pour réinitialiser. Pas de temps de rendre la position dynamique
            DrawRectangle(400, 50, 50, 50, Color.RED);
            DrawText("RESET", 400, 68, 15, Color.WHITE);

            // Les titres "CURRENT" et "BEST" sont déclarés sur DessinTableau pour que l'affichage soit dinamique
        }

        // Si le bouton arrière ou suivante est touché, on change de niveau
        static private void estBtnArriereSuivanteTouche(Vector2 mPos, ref int game_state, ref int coups, ref Bouton[,] level)
        {
            // Pas de temps de rendre dynamique la position des boutons
            
            // On regarde si le bouton arrière/suivante était touché
            if (Raylib.CheckCollisionPointTriangle(mPos, new Vector2(30, 70), new Vector2(65, 105), new Vector2(65, 45)))
            {
                game_state--;
                // 0 pour le menu. Plus grand pour dire si on se retrouve dans un niveau
                if (game_state > 0)
                    modifierTableauNiveau(game_state, ref level);
                coups = 0;
            }

            if (Raylib.CheckCollisionPointTriangle(mPos, new Vector2(535, 105), new Vector2(570, 70), new Vector2(535, 45)))
            {
                // Si on dans le dernière niveau, on fait rien
                if (game_state < 7)
                {
                    game_state++;
                    modifierTableauNiveau(game_state, ref level);
                    coups = 0;
                }
            }
        }
        
        // Si le bouton réinitialiser est touché, on fait apelle à modifierTableauNiveau
        static private void estBoutonReinitialiserToucher(Vector2 mPos, ref int game_state, ref int coups, ref Bouton[,] level)
        {
            // Pas de temps de rendre dynamique la position du bouton

            // On regarde si le bouton réinitialiser était touché
            if (CheckCollisionPointRec(mPos, new Rectangle(400, 50, 50, 50)))
            {
                modifierTableauNiveau(game_state, ref level);
                coups = 0;
            }
        }

        // Dessin de notre tableau de niveau
        static private void DessinTableauNiveau(ref Bouton[,] level, ref int coups, ref int best)
        {           
            // Pas de temps de rendre dynamique la position sur l'interface

            // Chaque bouton a une mésure de 100x100
            // Leurs bourdures est de +7 dans l'y est dans le x, alors le remplissage est de 86x86

            // Dessin des bouton arrière et suivante
            menuSuperieur();

            // On ajoute au texte les variables
            string s_coups = "CURRENT: " + coups.ToString();
            string s_best = "BEST: " + best.ToString(); 

            DrawText(s_coups, 80, 45, 25, MyGRAY);
            DrawText(s_best, 80, 85, 25, MyGRAY);

            // Génération de tableau
            int pos_x;
            int pos_y = 50;
            for (int y = 0; y <level.GetLength(0); y++)
            {
              // On ajoute 110 pour ajouter la distance d'un carré plus l'espacement
                pos_y += 110;
                // 30 pour le marge
                pos_x = 30;
                for (int x = 0; x < level.GetLength(1); x++)
                {
                    // Bordures carrés
                    if (level[y, x].couleur != Couleurs.VIDE)
                        DrawRectangle(pos_x, pos_y, 100, 100, level[y, x].getCouleur());

                    // Boutons spéciaux
                    // Cercles
                    if (level[y, x].type == 1)
                    {
                        DrawCircle(pos_x + 50, pos_y + 50, 40, Color.WHITE);
                        DrawCircle(pos_x + 50, pos_y + 50, 32, level[y, x].getCouleur());
                    }

                    // Bombe
                    else if (level[y, x].type == 2)
                    {
                        DrawCircle(pos_x + 50, pos_y + 50, 20, Color.WHITE);
                        DrawCircle(pos_x + 65, pos_y + 35, 5, Color.WHITE);
                        DrawLineEx(new Vector2(pos_x + 65, pos_y + 35), new Vector2(pos_x + 75, pos_y + 20), 3, Color.WHITE); 
                    }

                    // Flèche TO-DO changer dessin
                    else if (level[y, x].type == 3 || level[y, x].type == 5) 
                    {
                        if (level[y, x].direction == 1)
                        {
                            DrawLineEx(new Vector2(pos_x + 20, pos_y + 80), new Vector2(pos_x + 51, pos_y + 20), 10, Color.WHITE); 
                            DrawLineEx(new Vector2(pos_x + 80, pos_y + 80), new Vector2(pos_x + 49, pos_y + 20), 10, Color.WHITE); 
                        }
                        if (level[y, x].direction == 2)
                        {
                            DrawLineEx(new Vector2(pos_x + 20, pos_y + 20), new Vector2(pos_x + 80, pos_y + 51), 10, Color.WHITE); 
                            DrawLineEx(new Vector2(pos_x + 20, pos_y + 80), new Vector2(pos_x + 80, pos_y + 49), 10, Color.WHITE); 
                        }
                        if (level[y, x].direction == 3)
                        {
                            DrawLineEx(new Vector2(pos_x + 51, pos_y + 80), new Vector2(pos_x + 20, pos_y + 20), 10, Color.WHITE); 
                            DrawLineEx(new Vector2(pos_x + 49, pos_y + 80), new Vector2(pos_x + 81, pos_y + 20), 10, Color.WHITE); 
                        }
                        if (level[y, x].direction == 4)
                        {
                            DrawLineEx(new Vector2(pos_x + 80, pos_y + 20), new Vector2(pos_x + 20, pos_y + 51), 10, Color.WHITE); 
                            DrawLineEx(new Vector2(pos_x + 80, pos_y + 80), new Vector2(pos_x + 20, pos_y + 49), 10, Color.WHITE); 
                        }
                        if (level[y,x].type == 5)
                        {
                            DrawRectangle(pos_x + 15, pos_y + 60, 5, 25, Color.WHITE);
                            DrawRectangle(pos_x + 15, pos_y + 80, 15, 5, Color.WHITE);
                            DrawTriangle(
                            new Vector2(pos_x + 10, pos_y + 60),
                            new Vector2(pos_x + 25, pos_y + 60),
                            new Vector2(pos_x + 18, pos_y + 52), Color.WHITE);

                        }
                    }

                    else if (level[y, x].type == 4)
                    {
                        // Carré blanc pour remplir
                        DrawRectangle(pos_x + 7, pos_y + 7, 86, 86, Color.WHITE);

                        if (level[y, x].remplisage != Couleurs.VIDE)
                            DrawRectangle(pos_x + 10, pos_y + 10, 80, 80, level[y, x].getCouleurRem());
                    }
                    pos_x += 110;
                }
            }
        }

        // Ceci est exécute quand le joueur est dans un niveau est fait clique. C'est ici qu'on va réagir selon la case touché
        static private void JoueurFaitClique(ref Bouton[,] level, ref int game_state, ref int coups, ref int best)
        {
            // Pas de temps de rendre dynamique la position sur l'interface
            
            if (!win(ref level) && IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
            {
                Vector2 mPos = GetMousePosition();

                estBtnArriereSuivanteTouche(mPos, ref game_state, ref coups, ref level);
                
                estBoutonReinitialiserToucher(mPos, ref game_state, ref coups, ref level);

                // X
                // Moins 30 parce que le marge est de cette distance
                int indexI = Convert.ToInt32(mPos.X) - 30;
                int CellX;
                // Entre 0 et 550 pour nous retrouver dans les boutons est pas dehors
                if (indexI >= 0 && indexI <= 550)
                {
                    CellX = indexI / 110; // 100 pour le bouton et 10 pour le space
                    int spacingStartX = CellX * 110 + 100; // On calcule le début de la coordoné X

                    // On regarde si la souris est sur un space X et on returne -1
                    if (indexI >= spacingStartX && indexI <= spacingStartX + 10)
                        CellX = -1;
                }
                else
                    CellX = -1;

                // Y
                int indexJ = Convert.ToInt32(mPos.Y) - 160;
                int CellY;  
                if (indexJ >= 0 && indexJ <= 770)
                {
                    CellY = indexJ / 110; // 100 pour le bouton et 10 pour le space
                    int spacingStartY = CellY * 110 + 100; // On calcule le début de la coordoné Y

                    // On regarde si la souris est sur un space Y et on returne -1
                    if (indexJ >= spacingStartY && indexJ <= spacingStartY + 10)
                        CellY = -1;
                }
                else
                    CellY = -1;

                // Intéraction
                if (CellX != -1 && CellY != -1 && CellY < level.GetLength(0))
                {
                    // On doit contabiliser les coups (si on touche une case pas vide)
                    if (level[CellY, CellX].type != 0 && level[CellY, CellX].type != 4 )
                    {
                        BeginDrawing();
                        coups++;
                        DessinTableauNiveau(ref level, ref coups, ref best);
                        EndDrawing();
                    }

                    // Cercles
                    if (level[CellY, CellX].type == 1)
                    {
                      // S'il y a des cases à remplir, on va les remplir
                        if (!existeCasesRemplir(ref level, CellY, CellX))
                        {
                            remplissage4(ref level, CellY, CellX, level[CellY, CellX].getCouleurBouton(), Couleurs.VIDE, ref coups, ref best);
                        }
                        // On va les vider
                        else
                        {
                            level[CellY, CellX].remplisage = Couleurs.VIDE;
                            remplissage4(ref level, CellY, CellX, Couleurs.VIDE, level[CellY, CellX].getCouleurBouton(), ref coups, ref best);
                            level[CellY, CellX].remplisage = level[CellY, CellX].couleur;
                        }
                    }

                    // Bombe
                    if (level[CellY, CellX].type == 2)
                    {
                        for (int y = CellY -1; y <CellY +2; y++)
                            for (int x = CellX -1; x < CellX + 2; x++)
                            {
                                try
                                {
                                    if (level[y, x].type != 0)
                                    {
                                        // On convérti les boutons en carré normal
                                        level[y, x].type = 4;
                                        level[y, x].direction = 0;
                                        level[y, x].remplisage = level[CellY, CellX].remplisage;
                                    }
                                } catch 
                                {
                                }
                            }
                    }

                    // Flèches ET flèches tournants
                    if (level[CellY, CellX].type == 3 || level[CellY, CellX].type == 5)
                    {
                        // Flèches verticales
                        if (level[CellY, CellX].direction == 1 || level[CellY, CellX].direction == 3)
                        {
                            // Déclaration de somme pour aller en bas ou en haut selon la diréction
                            int somme = -1;
                            if (level[CellY, CellX].direction == 3)
                                somme = 1;

                            // Vider    Try catch dans le cas où on se retrouve avec une flèche qui regarde à l'éxterireur
                            try
                            {
                                if (level[CellY + somme, CellX].remplisage == level[CellY, CellX].remplisage)
                                {
                                    for (int y = CellY + somme; y >= 0; y = y + somme)
                                    {
                                        if (y == 6)
                                            break;
                                        if (level[y, CellX].remplisage != level[CellY, CellX].remplisage || level[y, CellX].type != 4)
                                            break;

                                        // BeginDrawing pour vider dinamiquement
                                        BeginDrawing();
                                        level[y, CellX].remplisage = Couleurs.VIDE;
                                        DessinTableauNiveau(ref level, ref coups, ref best);
                                        EndDrawing();
                                    }
                                }

                                // Remplir
                                else
                                {
                                    for (int y = CellY + somme; y >= 0; y = y + somme)
                                    {
                                        if (y == 6)
                                            break;
                                        if (level[y, CellX].type == 0 || level[y, CellX].remplisage != Couleurs.VIDE)
                                            break;

                                        BeginDrawing();
                                        level[y, CellX].remplisage = level[CellY, CellX].remplisage;
                                        DessinTableauNiveau(ref level, ref coups, ref best);
                                        EndDrawing();
                                    }
                                }
                            } catch { }
                        }

                        // Flèches horizontales
                        if (level[CellY, CellX].direction == 2 || level[CellY, CellX].direction == 4)
                        {
                            // Déclaration de somme pour aller à droite ou à gauche
                            int somme = -1;
                            if (level[CellY, CellX].direction == 2)
                                somme = 1;

                            // Vider
                            try
                            {

                                if (level[CellY, CellX + somme].remplisage == level[CellY, CellX].remplisage)
                                {
                                    for (int x = CellX + somme; x >= 0; x = x + somme)
                                    {
                                        if (x == 5)
                                            break;
                                        if (level[CellY, x].remplisage != level[CellY, CellX].remplisage || level[CellY, x].type != 4)
                                            break;

                                        BeginDrawing();
                                        level[CellY, x].remplisage = Couleurs.VIDE;
                                        DessinTableauNiveau(ref level, ref coups, ref best);
                                        EndDrawing();

                                    }
                                }

                                // Remplir

                                else
                                {
                                    for (int x = CellX + somme; x >= 0; x = x + somme)
                                    {
                                        if (x == 5)
                                            break;
                                        if (level[CellY, x].type == 0 || level[CellY, x].remplisage != Couleurs.VIDE)
                                            break;

                                        BeginDrawing();
                                        level[CellY, x].remplisage = level[CellY, CellX].remplisage;
                                        DessinTableauNiveau(ref level, ref coups, ref best);
                                        EndDrawing();
                                    }
                                }
                            }
                            catch { }
                        }

                        // Flèches tournants
                        if (level[CellY, CellX].type == 5)
                        {
                            if (level[CellY, CellX].direction == 4)
                                level[CellY, CellX].direction = 1;
                            else
                                level[CellY, CellX].direction += 1;
                        }
                    }
                }
            }
        }

        // Fonction qui cherche la donnée de meilleur score par niveau
        static private void bestScoreNiveau(ref int game_state, ref int best)
        {
            string jsonfileName = "levels.json";
            string jsonString = File.ReadAllText(jsonfileName);
            JsonNode niveaujson = JsonNode.Parse(jsonString)!;
            int c_best = 0;


            if (niveaujson is JsonObject jsonObject)
            {
                if (jsonObject.ContainsKey("levels") && niveaujson["levels"] is JsonArray levelsArray)
                {
                    foreach (var levelData in levelsArray)
                    {
                        if (levelData is JsonObject levelObject && levelObject.ContainsKey("level"))
                        {
                            int currentLevel = levelObject["level"]!.GetValue<int>();
                            if (currentLevel == game_state && levelObject.ContainsKey("data") && levelObject["data"] is JsonObject dataObject)
                            {
                                if (dataObject.ContainsKey("best"))
                                {
                                    c_best = dataObject["best"].GetValue<int>();
                                    best = c_best;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        
        // Fonction qui cherche dans le json des donées pour créer notre tableau niveauX
        static private Bouton[,] creerTableauNiveau(int nb_level)
        {
            // Tableau pour chaque niveau
            string jsonfileName = "levels.json";
            string json = File.ReadAllText(jsonfileName);

            RootObject root = JsonConvert.DeserializeObject<RootObject>(json);

            // Moins 1 pour le niveau équivalent
            Level firstLevel = root.levels[nb_level - 1];
            LevelData levelData = firstLevel.data;

            int numRows = levelData.couleur.Count;
            int numCols = levelData.couleur[0].Length;

            Bouton[,] level = new Bouton[numRows, numCols];

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    char couleurChar = levelData.couleur[i][j];
                    Couleurs couleur = (Couleurs)int.Parse(couleurChar.ToString());
                    int type = int.Parse(levelData.type[i][j].ToString());
                    int direction = int.Parse(levelData.direction[i][j].ToString());
                    char remplissageChar = levelData.remplissage[i][j];
                    Couleurs remplissage = (Couleurs)int.Parse(remplissageChar.ToString());

                    level[i, j] = new Bouton(couleur, type, direction, remplissage);
                }
            }

            return level;
        }
        
        // Fonction qui va modifier le tableau du niveau en insérent les données du niveau correspondant
        static private void modifierTableauNiveau(int nb_level, ref Bouton[,] level)
        {
            // Tableau pour chaque niveau
            string jsonfileName = "levels.json";
            string json = File.ReadAllText(jsonfileName);

            RootObject root = JsonConvert.DeserializeObject<RootObject>(json);

            Level firstLevel = root.levels[nb_level - 1];
            LevelData levelData = firstLevel.data;

            int numRows = levelData.couleur.Count;
            int numCols = levelData.couleur[0].Length;

            level = new Bouton[numRows, numCols];

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    char couleurChar = levelData.couleur[i][j];
                    Couleurs couleur = (Couleurs)int.Parse(couleurChar.ToString());
                    int type = int.Parse(levelData.type[i][j].ToString());
                    int direction = int.Parse(levelData.direction[i][j].ToString());
                    char remplissageChar = levelData.remplissage[i][j];
                    Couleurs remplissage = (Couleurs)int.Parse(remplissageChar.ToString());

                    level[i, j] = new Bouton(couleur, type, direction, remplissage);
                }
            }
        }
    }
}
