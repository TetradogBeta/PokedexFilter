using Gabriel.Cat.S.Extension;
using Gabriel.Cat.S.Utilitats;
using PokemonGBAFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace PokedexFilter2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static RomGba RomActual { get;  set; }
        public static NombreTipo[] Tipos { get; set; }

        Llista<Pokemon> filtroActual;
        int posActual;
        Thread hiloKeyDown;
        private bool ejecutarCambio;

        public MainWindow()
        {

            ContextMenu ctxMenu = new ContextMenu();
            MenuItem item = new MenuItem();
          
            InitializeComponent();
            posActual = 0;
            item.Header = "Cambiar rom";
            item.Click += (s, e) => PideRom();
            ctxMenu.Items.Add(item);
            ContextMenu = ctxMenu;
            //pongo los pkm :D
            PideRom();
            cmbPokemons.SelectedIndex = 0;
            gPokemonPrincipal.Espalda = false;
        }

        private void PideRom()
        {
            Label lblItemList;
            Microsoft.Win32.OpenFileDialog opnRom = new Microsoft.Win32.OpenFileDialog();
            SortedList<int, int> gruposHuevo = new SortedList<int, int>();
            opnRom.Filter = "GBA|*.gba";
            do
            {
                if (opnRom.ShowDialog().GetValueOrDefault())
                {
                    RomActual = new RomGba(opnRom.FileName);
                    Tipos = NombreTipo.Get(RomActual);
                    filtroActual = new Llista<Pokemon>(Pokemon.GetOrdenNacional(RomActual).Filtra((p)=>p.Descripcion.Altura!=default));
                    //filtroActual.Sort();
                    lstTipos.Items.Clear();
                    for (int i = 0; i < Tipos.Length; i++)
                    {
                        lblItemList = new Label();
                        lblItemList.Tag = i;
                        lblItemList.Content = Tipos[i];
                        lstTipos.Items.Add(lblItemList);
                    }
                    lstGruposHuevo.Items.Clear();
                    for (int i = 0, f = Enum.GetNames(typeof(Stats.GrupoHuevo)).Length; i < f; i++)
                    {
                        lblItemList = new Label();
                        lblItemList.Tag = i;
                        lblItemList.Content = (Stats.GrupoHuevo)i;
                        lstGruposHuevo.Items.Add(lblItemList);
                        gruposHuevo.Add(i, i);
                    }
                    for (int i = 0; i < filtroActual.Count; i++)
                    {
                        if (!gruposHuevo.ContainsKey((int)filtroActual[i].Stats.GrupoHuevo1))
                        {
                            lblItemList = new Label();
                            lblItemList.Tag = (int)filtroActual[i].Stats.GrupoHuevo1;
                            lblItemList.Content = "#" + (int)filtroActual[i].Stats.GrupoHuevo1;
                            lstGruposHuevo.Items.Add(lblItemList);
                            gruposHuevo.Add((int)filtroActual[i].Stats.GrupoHuevo1, (int)filtroActual[i].Stats.GrupoHuevo1);
                        }
                        if (!gruposHuevo.ContainsKey((int)filtroActual[i].Stats.GrupoHuevo2))
                        {
                            lblItemList = new Label();
                            lblItemList.Tag = (int)filtroActual[i].Stats.GrupoHuevo2;
                            lblItemList.Content = "#" + (int)filtroActual[i].Stats.GrupoHuevo2;
                            lstGruposHuevo.Items.Add(lblItemList);
                            gruposHuevo.Add((int)filtroActual[i].Stats.GrupoHuevo2, (int)filtroActual[i].Stats.GrupoHuevo2);
                        }
                    }
                    cmbPokemons.ItemsSource = filtroActual;
                    PonPokemons();
                }

            } while (RomActual == null && MessageBox.Show("No se puede hacer nada sin rom...¿quieres volver a buscar?", "Se requiere ROM", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes);
            if (RomActual == null)
                this.Close();
        }

        private void PonPokemons()
        {
            gPokemonUp1.Pokemon = filtroActual.GetElementActual(Ordre.Consecutiu, posActual - 5);
            gPokemonUp2.Pokemon = filtroActual.GetElementActual(Ordre.Consecutiu, posActual - 4);
            gPokemonUp3.Pokemon = filtroActual.GetElementActual(Ordre.Consecutiu, posActual - 3);
            gPokemonUp4.Pokemon = filtroActual.GetElementActual(Ordre.Consecutiu, posActual - 2);

            gPokemonMedioUp.Pokemon = filtroActual.GetElementActual(Ordre.Consecutiu, posActual - 1);

            gPokemonPrincipal.Pokemon = filtroActual[posActual];

            gPokemonMedioDown.Pokemon = filtroActual.GetElementActual(Ordre.Consecutiu, posActual + 1);

            gPokemonDown1.Pokemon = filtroActual.GetElementActual(Ordre.Consecutiu, posActual + 2);
            gPokemonDown2.Pokemon = filtroActual.GetElementActual(Ordre.Consecutiu, posActual + 3);
            gPokemonDown3.Pokemon = filtroActual.GetElementActual(Ordre.Consecutiu, posActual + 4);
            gPokemonDown4.Pokemon = filtroActual.GetElementActual(Ordre.Consecutiu, posActual + 5);

        }
        //moverse :D
        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            bool isControlPress = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
            if (e.Delta < 0)
            {
                if (isControlPress)
                    posActual += 9;
                posActual++;
            }
            else
            {
                if (isControlPress)
                    posActual -= 9;
                posActual--;
            }
            if (posActual < 0)
            {
                posActual = filtroActual.Count - 1;
            }
            posActual = posActual % filtroActual.Count;
            PonPokemons();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (hiloKeyDown == null)
                if (e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Left || e.Key == Key.Right)
                {
                    if (hiloKeyDown != null)
                    {
                        hiloKeyDown.Abort();
                        hiloKeyDown = null;
                    }
                    hiloKeyDown = new Thread(() => MueveListaConTeclado());
                    hiloKeyDown.Start();
                }
        }
        private void MueveListaConTeclado()
        {
            Action act = () => MueveListaConTecladoInvoked();
            ejecutarCambio = true;
            while (true)
            {

                Dispatcher.BeginInvoke(act);
                Thread.Sleep(100);
            }
        }
        private void MueveListaConTecladoInvoked()
        {
            bool isControlPress = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
            if (Keyboard.IsKeyDown(Key.Down) || Keyboard.IsKeyDown(Key.Right))
            {
                if (isControlPress)
                    posActual += 9;
                posActual++;
            }
            else
            {
                if (isControlPress)
                    posActual -= 9;
                posActual--;
            }
            if (posActual < 0)
            {
                posActual = filtroActual.Count + posActual;
            }
            posActual = posActual % filtroActual.Count;
            if (ejecutarCambio)
                PonPokemons();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Left || e.Key == Key.Right)
            {
                ejecutarCambio = false;
                if (hiloKeyDown != null)
                {
                    hiloKeyDown.Abort();
                    hiloKeyDown = null;
                }
            }
        }

        private void Filtra(object sender, RoutedEventArgs e)
        {
            SortedList<int, int> tipos = new SortedList<int, int>();
            SortedList<int, int> gruposHuevo = new SortedList<int, int>();
            int aux;
            posActual = 0;
            for (int i = 0; i < lstGruposHuevo.SelectedItems.Count; i++)
            {
                aux = (int)((Label)lstGruposHuevo.SelectedItems[i]).Tag;
                gruposHuevo.Add(aux, aux);
            }
            for (int i = 0; i < lstTipos.SelectedItems.Count; i++)
            {
                aux = (int)((Label)lstTipos.SelectedItems[i]).Tag;
                tipos.Add(aux, aux);
            }
            filtroActual = new Llista<Pokemon>(Pokemon.GetOrdenNacional(RomActual).Filtra((p) => p.Descripcion.Altura != default).Filtra((pokemon) => { return (gruposHuevo.Count == 0 || gruposHuevo.ContainsKey((int)pokemon.Stats.GrupoHuevo1) || gruposHuevo.ContainsKey((int)pokemon.Stats.GrupoHuevo2)) && (tipos.Count == 0 || tipos.ContainsKey((int)pokemon.Stats.Tipo1) || tipos.ContainsKey((int)pokemon.Stats.Tipo2)); }));
            if (filtroActual.Count == 0)
            {
                filtroActual = new Llista<Pokemon>();
                filtroActual.Add(Pokemon.Get(RomActual,0));

            }
            //filtroActual.Sort();
            cmbPokemons.ItemsSource = filtroActual;
            posActual = 0;
            PonPokemons();
        }

        private void cmbPokemons_Selected(object sender, RoutedEventArgs e)
        {
            if (cmbPokemons.SelectedIndex >= 0)
            {
                posActual = cmbPokemons.SelectedIndex;
                PonPokemons();
            }
        }
    }
}
