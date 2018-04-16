using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox
{
    public abstract class BaseMenu : MonoBehaviour, IMenu
    {
        protected int currentIndexSelection = 0;

        public int CurrentIndexSelection
        {
            get { return currentIndexSelection; }
            set
            {
                // Modifiche grafiche per cambiare colore alla nuova selezione e far tornare la vecchia selezione al colore precedente.
                currentIndexSelection = value;
                for (int i = 0; i < _selectableButtons.Count; i++)
                {
                    if (_selectableButtons[i].Index == value)
                    {
                        _selectableButtons[i].IsSelected = true;
                    }
                    else { _selectableButtons[i].IsSelected = false; }
                }
            }
        }

        protected List<ISelectable> _selectableButtons = new List<ISelectable>();

        public List<ISelectable> SelectableButtons
        {
            get { return _selectableButtons; }
            set { _selectableButtons = value; }
        }

        Player player;

        public Player Player
        {
            get { return player; }
            set { player = value; }
        }

        /// <summary>
        /// Salva all'interno della lista SelectableButton tutti i bottoni con attaccato ISelectable, gli assegna un index
        /// </summary>
        public virtual void FindISelectableChildren()
        {
            foreach (ISelectable item in GetComponentsInChildren<ISelectable>())
            {
                SelectableButtons.Add(item);
            }

            for (int i = 0; i < _selectableButtons.Count; i++)
            {
                _selectableButtons[i].SetIndex(i);
            }
        }

        #region Menu Actions
        public virtual void GoDownInMenu(Player _player)
        {
            if (SelectableButtons.Count > 0)
                CurrentIndexSelection++;
            if (CurrentIndexSelection > SelectableButtons.Count - 1)
                CurrentIndexSelection = 0;
        }

       
        public virtual void GoUpInMenu(Player _player)
        {
            if (SelectableButtons.Count > 0)
                CurrentIndexSelection--;
            if (CurrentIndexSelection < 0)
                CurrentIndexSelection = SelectableButtons.Count - 1;
        }

        public virtual void GoRightInMenu(Player _player)
        {
            
        }

        public virtual void GoLeftInMenu(Player _player)
        {

        }

        /// <summary>
        /// Funzione che in base all'override esegue la funzione del bottone attualmente selezionato
        /// </summary>
        public virtual void Selection(Player _player) { }

        public virtual void GoBack(Player _player) { }
        #endregion
    }
}