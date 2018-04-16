using System.Collections;
using System.Collections.Generic;

namespace BlackFox {
    public interface IMenu
    {
        Player Player { get; set; }

        int CurrentIndexSelection { get; set; }
       
        List<ISelectable> SelectableButtons { get; set; }

        /// <summary>
        /// Chiama la funzione per cambiare lo stato della StateMachine
        /// </summary>
        void Selection(Player _player);

        /// <summary>
        /// Sposta l'indice della selezione indietro
        /// </summary>
        void GoUpInMenu(Player _player);
              
        /// <summary>
        /// Sposta l'indice della selezione in avanti
        /// </summary>
        void GoDownInMenu(Player _player);

        /// <summary>
        /// Sposta l'indice della selezione a destra
        /// </summary>
        void GoRightInMenu(Player _player);

        /// <summary>
        /// Sposta l'indice della selezione a sinistra
        /// </summary>
        void GoLeftInMenu(Player _player);
    }

    /// <summary>
    /// Interfaccia per tutti gli oggetti selezionabili.
    /// </summary>
    public interface ISelectable
    {
        int Index{ get; set; }
        bool IsSelected { get; set; }
        void SetIndex(int _index);
        void CheckIsSelected(bool _isSelected);
    }
}