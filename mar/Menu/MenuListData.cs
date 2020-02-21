using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace mar
{
    public class MenuListData : List<MenuItem>
    {
        public MenuListData()
        {
            this.Add(new MenuItem()
            {
                Titulo = "Home",
                TargetType = typeof(Home)
            });

            this.Add(new MenuItem()
            {
                Titulo = "Perfil",
                TargetType = typeof(Perfil)
            });

            this.Add(new MenuItem()
            {
                Titulo = "Promociones",
                TargetType = typeof(Promocion)
            });

            this.Add(new MenuItem()
            {
                Titulo = "Tips",
                TargetType = typeof(Tips)
            });

            this.Add(new MenuItem()
            {
                Titulo = "Pedidos",
                TargetType = typeof(Pedidos)
            });

            this.Add(new MenuItem()
            {
                Titulo = "Ayuda",
                TargetType = typeof(Contact)
            });

            this.Add(new MenuItem()
            {
                Titulo = "Configuración",
                TargetType = typeof(Configuraciones)
            });
        }
    }
}


