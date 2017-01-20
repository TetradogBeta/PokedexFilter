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
        public  static  SortedList<string, System.Windows.Media.Color> ColorTipo;

        Pokemon pokemon;
        bool espalda;
        BitmapAnimated bmpImgAnimated;

        private bool Animando { get; set; }
        
        static PokemonViewer()
        {
            ColorTipo = new SortedList<string, System.Windows.Media.Color>();

            ColorTipo.Add("ACERO", Colors.Gray);
            ColorTipo.Add("STEEL", Colors.Gray);

            ColorTipo.Add("AGUA", Colors.Blue);
            ColorTipo.Add("WATER", Colors.Blue);

            ColorTipo.Add("BICHO", Colors.YellowGreen);
            ColorTipo.Add("BUG", Colors.YellowGreen);

            ColorTipo.Add("DRAGÓN", Colors.MediumPurple);
            ColorTipo.Add("DRAGON", Colors.MediumPurple);

            ColorTipo.Add("ELÉCT.", Colors.Gold);
            ColorTipo.Add("ELECTR", Colors.Gold);

            ColorTipo.Add("FANT.", Colors.Purple);
            ColorTipo.Add("GHOST", Colors.Purple);

            ColorTipo.Add("FUEGO", Colors.OrangeRed);
            ColorTipo.Add("FIRE", Colors.OrangeRed);

            ColorTipo.Add("HIELO", Colors.AliceBlue);
            ColorTipo.Add("ICE", Colors.AliceBlue);

            ColorTipo.Add("LUCHA", Colors.DarkRed);
            ColorTipo.Add("FIGHT", Colors.DarkRed);

            ColorTipo.Add("NORMAL", Colors.LightGray);

            ColorTipo.Add("PLANTA", Colors.LightGreen);
            ColorTipo.Add("GRASS", Colors.LightGreen);

            ColorTipo.Add("PSÍQ.", Colors.LightPink);
            ColorTipo.Add("PSYCHC", Colors.LightPink);

            ColorTipo.Add("ROCA", Colors.SaddleBrown);
            ColorTipo.Add("ROCK", Colors.SaddleBrown);

            ColorTipo.Add("SINIE.", Colors.Brown);
            ColorTipo.Add("DARK", Colors.Brown);

            ColorTipo.Add("TIERRA", Colors.SandyBrown);
            ColorTipo.Add("GROUND", Colors.SandyBrown);

            ColorTipo.Add("VENENO", Colors.Pink);
            ColorTipo.Add("POISON", Colors.Pink);

            ColorTipo.Add("VOLAD.", Colors.SteelBlue);
            ColorTipo.Add("FLYING", Colors.SteelBlue);

            ColorTipo.Add("(?)", Colors.White);
            ColorTipo.Add("???", Colors.White);
        }
        public PokemonViewer() : this(null) { }
        public PokemonViewer(Pokemon pokemon, bool espalda=true)
        {
            Animando = false;
            InitializeComponent();
            if (pokemon != null)
            {
                this.Pokemon = pokemon;
                this.Espalda = espalda;
            }
           
        }
        public Pokemon Pokemon
        {
            get
            {
                return pokemon;
            }

            set
            {
                pokemon = value;
                if (pokemon != null)
                {
                    txtNombre.Text = "#" + pokemon.OrdenPokedexNacional + " " + pokemon.Nombre;
                    PonImagen();

                    try
                    {
                        gsColor1.Color = ColorTipo[MainWindow.RomActual.Tipos[pokemon.Tipo1].Nombre.ToString().ToUpper()];
                    }
                    catch { gsColor1.Color = Colors.White; }
                    try
                    {
                        gsColor2.Color = ColorTipo[MainWindow.RomActual.Tipos[pokemon.Tipo2].Nombre.ToString().ToUpper()];
                    }
                    catch { gsColor2.Color = Colors.Orange; }
                }
                if (bmpImgAnimated != null)
                {
                    if(Animando)
                    bmpImgAnimated.Finsh();
                    bmpImgAnimated.FrameChanged -= PonImagenAnimacion;
                }
                if(pokemon!=null)
                if (pokemon.Sprites != null)
                {
                    bmpImgAnimated = pokemon.Sprites.GetAnimacionImagenFrontal();
                    bmpImgAnimated.FrameASaltarAnimacionCiclica = 1;
                    bmpImgAnimated.AnimarCiclicamente = true;
                    bmpImgAnimated.FrameChanged += PonImagenAnimacion;
                    if (Animando)
                        bmpImgAnimated.Start();
                }
                
            }
        }

        private void PonImagen()
        {
            if (pokemon != null)
            {
                if (Espalda)
                {
                    img.SetImage(pokemon.Sprites.GetImagenTrasera());
                }
                else
                {
                    img.SetImage(pokemon.Sprites.GetImagenFrontal());
                }
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
            if (bmpImgAnimated != null)
            {
                bmpImgAnimated.Start();
                Animando = true;
            }

        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            //deja de moverse
            if (bmpImgAnimated != null)
            {
                bmpImgAnimated.Finsh();
                PonImagen();
                Animando = false;
            }
        }

        private void PonImagenAnimacion(BitmapAnimated bmpAnimated, Bitmap frameActual)
        {
            Action act=()=>img.SetImage(frameActual);
            Dispatcher.BeginInvoke(act);
        }
    }
}
