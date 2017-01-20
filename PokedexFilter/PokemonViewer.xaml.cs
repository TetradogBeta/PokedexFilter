using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PokemonGBAFrameWork;
using Gabriel.Cat.Extension;
using Gabriel.Cat;
using System.Drawing;

namespace PokedexFilter
{
    /// <summary>
    /// Lógica de interacción para PokemonViewer.xaml
    /// </summary>
    public partial class PokemonViewer : UserControl
    {
        static SortedList<string, System.Windows.Media.Color> colorTipo;

        Pokemon pokemon;
        bool espalda;
        BitmapAnimated bmpImgAnimated;

        private bool Animando { get; set; }
        
        static PokemonViewer()
        {
            colorTipo = new SortedList<string, System.Windows.Media.Color>();

            colorTipo.Add("ACERO", Colors.Gray);
            colorTipo.Add("STEEL", Colors.Gray);

            colorTipo.Add("AGUA", Colors.Blue);
            colorTipo.Add("WATER", Colors.Blue);

            colorTipo.Add("BICHO", Colors.YellowGreen);
            colorTipo.Add("BUG", Colors.YellowGreen);

            colorTipo.Add("DRAGÓN", Colors.MediumPurple);
            colorTipo.Add("DRAGON", Colors.MediumPurple);

            colorTipo.Add("ELÉCT.", Colors.Gold);
            colorTipo.Add("ELECTR", Colors.Gold);

            colorTipo.Add("FANT.", Colors.Purple);
            colorTipo.Add("GHOST", Colors.Purple);

            colorTipo.Add("FUEGO", Colors.OrangeRed);
            colorTipo.Add("FIRE", Colors.OrangeRed);

            colorTipo.Add("HIELO", Colors.AliceBlue);
            colorTipo.Add("ICE", Colors.AliceBlue);

            colorTipo.Add("LUCHA", Colors.DarkRed);
            colorTipo.Add("FIGHT", Colors.DarkRed);

            colorTipo.Add("NORMAL", Colors.LightGray);
            colorTipo.Add("NORMAL", Colors.LightGray);

            colorTipo.Add("PLANTA", Colors.LightGreen);
            colorTipo.Add("GRASS", Colors.LightGreen);

            colorTipo.Add("PSÍQ.", Colors.LightPink);
            colorTipo.Add("PSYCHC", Colors.LightPink);

            colorTipo.Add("ROCA", Colors.SaddleBrown);
            colorTipo.Add("ROCK", Colors.SaddleBrown);

            colorTipo.Add("SINIE.", Colors.Brown);
            colorTipo.Add("DARK", Colors.Brown);

            colorTipo.Add("TIERRA", Colors.SandyBrown);
            colorTipo.Add("GROUND", Colors.SandyBrown);

            colorTipo.Add("VENENO", Colors.Pink);
            colorTipo.Add("POISON", Colors.Pink);

            colorTipo.Add("VOLAD.", Colors.SteelBlue);
            colorTipo.Add("FLYING", Colors.SteelBlue);

            colorTipo.Add("(?)", Colors.White);
            colorTipo.Add("???", Colors.White);
        }
        public PokemonViewer(Pokemon pokemon, bool espalda=true)
        {
  
            Animando = false;
            InitializeComponent();
            this.Pokemon = pokemon;
            this.Espalda = espalda;
           
        }
        public Pokemon Pokemon
        {
            get
            {
                return pokemon;
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                pokemon = value;
                PonImagen();
                try
                {
                    gsColor1.Color = colorTipo[MainWindow.RomActual.Tipos[pokemon.Tipo1].Nombre.ToString().ToUpper()];
                }
                catch { gsColor1.Color = Colors.White; }
                try
                {
                    gsColor2.Color = colorTipo[MainWindow.RomActual.Tipos[pokemon.Tipo2].Nombre.ToString().ToUpper()];
                }
                catch { gsColor2.Color = Colors.Black; }

                if (bmpImgAnimated != null)
                {
                    if(Animando)
                    bmpImgAnimated.Finsh();
                    bmpImgAnimated.FrameChanged -= PonImagenAnimacion;
                }
                bmpImgAnimated = pokemon.Sprites.GetAnimacionImagenFrontal();
                bmpImgAnimated.AnimarCiclicamente = true;
                bmpImgAnimated.FrameChanged += PonImagenAnimacion;
                if (Animando)
                    bmpImgAnimated.Start();
            }
        }

        private void PonImagen()
        {
           if(Espalda)
            {
                img.SetImage(pokemon.Sprites.GetImagenTrasera());
            }
           else
            {
                img.SetImage(pokemon.Sprites.GetImagenFrontal());
            }
        }

        public bool Espalda
        {
            get
            {
                return espalda;
            }

            set
            {
                espalda = value;
                PonImagen();
            }
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            //se mueve :D
            bmpImgAnimated.Start();
            Animando = true;

        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            //deja de moverse
            bmpImgAnimated.Finsh();
            PonImagen();
            Animando = false;
        }

        private void PonImagenAnimacion(BitmapAnimated bmpAnimated, Bitmap frameActual)
        {
            img.SetImage(frameActual);
        }
    }
}
