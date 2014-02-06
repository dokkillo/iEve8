using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página de elementos agrupados está documentada en http://go.microsoft.com/fwlink/?LinkId=234231

namespace iEve8
{
    /// <summary>
    /// Página en la que muestra una colección de elementos agrupados.
    /// </summary>
    public sealed partial class CharacterPage : iEve8.Common.LayoutAwarePage
    {
        public CharacterPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Rellena la página con el contenido pasado durante la navegación. Cualquier estado guardado se
        /// proporciona también al crear de nuevo una página a partir de una sesión anterior.
        /// </summary>
        /// <param name="navigationParameter">Valor de parámetro pasado a
        /// <see cref="Frame.Navigate(Type, Object)"/> cuando se solicitó inicialmente esta página.
        /// </param>
        /// <param name="pageState">Diccionario del estado mantenido por esta página durante una sesión
        /// anterior. Será null la primera vez que se visite una página.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            //this.DefaultViewModel["Items"]
            //var character = 
            // TODO: Asignar una colección de grupos enlazables a  this.DefaultViewModel["Groups"]
        }
    }
}
