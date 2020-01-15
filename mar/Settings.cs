using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;

namespace mar
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters.
	/// </summary>
	public static class Settings
	{


		private static ISettings AppSettings =>
		CrossSettings.Current;

		public static string Idusuario
		{
			get => AppSettings.GetValueOrDefault(nameof(Idusuario), string.Empty);
			set => AppSettings.AddOrUpdateValue(nameof(Idusuario), value);
		}

        public static string Pedido
        {
            get => AppSettings.GetValueOrDefault(nameof(Pedido), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(Pedido), value);
        }

        public static string Categoria
        {
            get => AppSettings.GetValueOrDefault(nameof(Categoria), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(Categoria), value);
        }
    }
}
