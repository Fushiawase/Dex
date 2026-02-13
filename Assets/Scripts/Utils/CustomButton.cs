using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace App
{
    public class CustomButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image background;

        public bool selected;

        public void Select(bool selected)
        {
            this.selected = selected;
            background.color = selected ? Color.yellow : Color.white;
        }

        public void DefineBehaviour(UnityAction action) => button.onClick.AddListener(action);
    }
}