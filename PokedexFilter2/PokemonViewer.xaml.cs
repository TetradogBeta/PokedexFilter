using Gabriel.Cat.S.Extension;
using Gabriel.Cat.S.Utilitats;
using PokemonGBAFramework.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PokedexFilter2
{
    /// <summary>
    /// Lógica de interacción para PokemonViewer.xaml
    /// </summary>
    public partial class PokemonViewer : UserControl
    {
        public static TwoKeysList<int, string, System.Windows.Media.Color> ColorTipo;

        Pokemon pokemon;
        bool espalda;
        BitmapAnimated bmpImgAnimated;

        private bool Animando { get; set; }

        static PokemonViewer()
        {
            ColorTipo = new TwoKeysList<int,string, System.Windows.Media.Color>();

            ColorTipo.Add(8,"ACERO", Colors.Gray);


            ColorTipo.Add(11,"AGUA", Colors.Blue);


            ColorTipo.Add(6,"BICHO", Colors.YellowGreen);


            ColorTipo.Add(16,"DRAGÓN", Colors.MediumPurple);


            ColorTipo.Add(13,"ELÉCT.", Colors.Gold);


            ColorTipo.Add(7,"FANT.", Colors.Purple);


            ColorTipo.Add(10,"FUEGO", Colors.OrangeRed);


            ColorTipo.Add(15,"HIELO", Colors.AliceBlue);


            ColorTipo.Add(1,"LUCHA", Colors.DarkRed);


            ColorTipo.Add(0,"NORMAL", Colors.LightGray);

            ColorTipo.Add(12,"PLANTA", Colors.LightGreen);


            ColorTipo.Add(14,"PSÍQ.", Colors.LightPink);


            ColorTipo.Add(5,"ROCA", Colors.SaddleBrown);


            ColorTipo.Add(17,"SINIE.", Colors.Brown);


            ColorTipo.Add(4,"TIERRA", Colors.SandyBrown);


            ColorTipo.Add(3,"VENENO", Colors.Pink);

            ColorTipo.Add(2,"VOLAD.", Colors.SteelBlue);

            ColorTipo.Add(9,"(?)", Colors.White);

        }
        public PokemonViewer() : this(null) { }
        public PokemonViewer(Pokemon pokemon, bool espalda = true)
        {
            Animando = false;
            InitializeComponent();
            if (pokemon != null)
            {
                this.Pokemon = pokemon;
                this.Espalda = espalda;
            }
            else { this.espalda = true; }

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
                    txtNombre.Text = "#" + pokemon.OrdenNacional + " " + pokemon.Nombre;
                    PonImagen();

                    try
                    {
                        gsColor1.Color = ColorTipo.GetValueWithKey1((int)pokemon.Stats.Tipo1);
                    }
                    catch { gsColor1.Color = Colors.White; }
                    try
                    {
                        gsColor2.Color = ColorTipo.GetValueWithKey1((int)pokemon.Stats.Tipo2);
                    }
                    catch { gsColor2.Color = Colors.Orange; }
                }
                if (bmpImgAnimated != null)
                {
                    if (Animando)
                        bmpImgAnimated.Stop();
                    bmpImgAnimated.FrameChanged -= PonImagenAnimacion;
                }
                if (pokemon != null)
                    if (pokemon.Sprites != null)
                    {
                        bmpImgAnimated = pokemon.Sprites.Frontales.GetAnimacionImagenFrontal(pokemon.Sprites.PaletaNomal);
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
                    img.SetImage((Bitmap)pokemon.Sprites.Traseros);
                }
                else
                {
                    img.SetImage((Bitmap)pokemon.Sprites.Frontales);
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
                bmpImgAnimated.Stop();
                PonImagen();
                Animando = false;
            }
        }

        private void PonImagenAnimacion(BitmapAnimated bmpAnimated, Bitmap frameActual)
        {
            Action act = () => img.SetImage(frameActual);
            Dispatcher.BeginInvoke(act);
        }
    }
}
