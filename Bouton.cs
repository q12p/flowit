using System;
using System.Collections.Generic;
//using System.Drawing;
using Raylib_cs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flowit
{
    enum Couleurs
    {
        VIDE,     // 0
        RED,      // 1
        YELLOW,   // 2
        GREEN,    // 3
        BLUE,     // 4
        GRAY      // 5
    }
    enum Remplisage
    {
        VIDE,
        RED,
        YELLOW,
        GREEN,
        BLUE,
        GRAY
    }
    
    public class LevelData
    {
    public List<string> couleur { get; set; }
    public List<string> type { get; set; }
    public List<string> direction { get; set; }
    public List<string> remplissage { get; set; }
    }

    public class Level
    {
    public int level { get; set; }
    public LevelData data { get; set; }
    public int best { get; set; }
    }

    public class RootObject
    {
    public List<Level> levels { get; set; }
    }


    internal class Bouton
    {
        public Couleurs couleur, remplisage;

        public int type;
        // Types :
        // 1 = Cercle
        // 2 = Bombe
        // 3 = Fl�che
        // 4 = Rien     Pas de modification
        // 5 = Fl�che TOURNANT                  !!
        // 0 = Vide     Pas de carr� du tout

        public int direction;
        // Direction :
        // 1 = Haut
        // 2 = Droite
        // 3 = Bas
        // 4 = Gauche
        // 0 = Aucune
        
        public Bouton(Couleurs couleur, int type, int direction, Couleurs remplisage)
        {
            this.couleur = couleur;
            this.type = type;
            this.direction = direction;
            this.remplisage = remplisage;

        }
        public Couleurs getCouleurBouton()
        {
            return this.couleur;
        }
        public Couleurs getRemplisageBouton()
        {
            return this.remplisage;
        }

        public Color getCouleurRem()
        {
            Color MyRED = new Color( 245, 21, 24, 255);
            Color MyYELLOW = new Color( 246, 202, 24, 255);
            Color MyGREEN = new Color( 104, 159, 56, 255);
            Color MyBLUE = new Color(104, 166, 229, 255);
            Color MyGRAY = new Color( 125, 106, 108, 255);

            if (this.remplisage == Couleurs.RED)
                return MyRED;
            if (this.remplisage == Couleurs.YELLOW)
                return MyYELLOW;
            if (this.remplisage == Couleurs.GREEN)
                return MyGREEN;
            if (this.remplisage == Couleurs.BLUE)
                return MyBLUE;
            if (this.remplisage == Couleurs.GRAY)
                return MyGRAY;
            else
                return Color.WHITE;
        }
        public Color getCouleur()
        {
            Color MyRED = new Color( 245, 21, 24, 255);
            Color MyYELLOW = new Color( 246, 202, 24, 255);
            Color MyGREEN = new Color( 104, 159, 56, 255);
            Color MyBLUE = new Color(104, 166, 229, 255);
            Color MyGRAY = new Color( 125, 106, 108, 255);

            if (this.couleur == Couleurs.RED)
                return MyRED;
            if (this.couleur == Couleurs.YELLOW)
                return MyYELLOW;
            if (this.couleur == Couleurs.GREEN)
                return MyGREEN;
            if (this.couleur == Couleurs.BLUE)
                return MyBLUE;
            if (this.couleur == Couleurs.GRAY)
                return MyGRAY;
            else
                return Color.WHITE;
        }
    }
}
