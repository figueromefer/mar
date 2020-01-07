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
                Titulo = "Perfil",
                TargetType = typeof(Perfil)
            });

            this.Add(new MenuItem()
            {
                Titulo = "Amigos",
                TargetType = typeof(Amigos)
            });

            this.Add(new MenuItem()
            {
                Titulo = "Mensajes",
                TargetType = typeof(Mensajes)
            });

            this.Add(new MenuItem()
            {
                Titulo = "Configuraciones",
                TargetType = typeof(Configuraciones)
            });

            this.Add(new MenuItem()
            {
                Titulo = "Cerrar sesión",
                TargetType = typeof(Logout)
            });
        }
    }
}


