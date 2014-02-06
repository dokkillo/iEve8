using iEve8.Data;
using iEve8Lib;
using iEve8Lib.BLL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página de elementos está documentada en http://go.microsoft.com/fwlink/?LinkId=234233

namespace iEve8
{
    /// <summary>
    /// Página en la que se muestra una colección de vistas previas de elementos. En la aplicación dividida, esta página
    /// se usa para mostrar y seleccionar uno de los grupos disponibles.
    /// </summary>
    public sealed partial class ItemsPage : iEve8.Common.LayoutAwarePage
    {
        List<Character> CharacterList;

        public ItemsPage()
        {
            this.InitializeComponent();
            CharacterList = new List<Character>();
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
            // TODO: Crear un modelo de datos adecuado para el dominio del problema para reemplazar los datos de ejemplo
            var sampleDataGroups = SampleDataSource.GetGroups((String)navigationParameter);
            EveAccounts oEve = new EveAccounts();
            CharacterList = oEve.getAccountCharacters("test");
            this.DefaultViewModel["Items"] = CharacterList;
           
            
           
        }

        /// <summary>
        /// Se invoca al hacer clic en un elemento.
        /// </summary>
        /// <param name="sender">Objeto GridView (o ListView cuando la aplicación está en estado Snapped)
        /// que muestra el elemento en el que se hizo clic.</param>
        /// <param name="e">Datos de evento que describen el elemento en el que se hizo clic.</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navegar a la página de destino adecuada y configurar la nueva página
            // al pasar la información requerida como parámetro de navegación
            var characterId = ((Character)e.ClickedItem).CharacterId;
            var character = (from t in CharacterList where t.CharacterId == characterId select t).FirstOrDefault();            
            this.Frame.Navigate(typeof(CharacterPage), character );
            
        }
    }
}
