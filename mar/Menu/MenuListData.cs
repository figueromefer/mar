using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Toss
{
    public class MenuListData : List<MenuItem>
    {
        public MenuListData()
        {
            this.Add(new MenuItem()
            {
                Titulo = "Home",
                TargetType = typeof(mar.Paginas.Home)
            });

            this.Add(new MenuItem()
            {
                Titulo = "Perfil",
                TargetType = typeof(mar.Perfil)
            });

            this.Add(new MenuItem()
            {
                Titulo = "Ayuda",
                //TargetType = typeof(Mensajes)
            });

            this.Add(new MenuItem()
            {
                Titulo = "Configuraciones",
                //TargetType = typeof(Configuraciones)
            });

            this.Add(new MenuItem()
            {
                Titulo = "Cerrar sesión",
                //TargetType = typeof(Logout)
            });
        }
    }
}


