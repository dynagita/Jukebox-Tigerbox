using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tigerbox.LetterPanel
{
    public partial class LetterPanel: UserControl
    {
        /// <summary>
        /// Check if letter panel is visible
        /// </summary>
        public bool IsVisible
        {
            get
            {
                return this._pnlLetters.Visible;
            }
        }

        //Default letter selected = A
        private int _selectedLetter = 0;

        //Labels Dictionary
        private Dictionary<int, string> _labelsDictionary;

        /// <summary>
        /// Get the selected letter
        /// </summary>
        private string SelectedLetter
        {
            get
            {
                return _labelsDictionary[_selectedLetter];
            }
        }

        /// <summary>
        /// Get DateTime's last operation into component
        /// </summary>
        public DateTime LastOperation { get; private set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public LetterPanel()
        {
            InitializeComponent();

            #region Letters Map
            _labelsDictionary = new Dictionary<int, string>
            {
                { 0, "A" },
                { 1, "B" },
                { 2, "C" },
                { 3, "D" },
                { 4, "E" },
                { 5, "F" },
                { 6, "G" },
                { 7, "H" },
                { 8, "I" },
                { 9, "J" },
                { 10, "K" },
                { 11, "L" },
                { 12, "M" },
                { 13, "N" },
                { 14, "O" },
                { 15, "P" },
                { 16, "Q" },
                { 17, "R" },
                { 18, "S" },
                { 19, "T" },
                { 20, "U" },
                { 21, "V" },
                { 22, "W" },
                { 23, "X" },
                { 24, "Y" },
                { 25, "Z" },
                { 26, "0" },
                { 27, "1" },
                { 28, "2" },
                { 29, "3" },
                { 30, "4" },
                { 31, "5" },
                { 32, "6" },
                { 33, "7" },
                { 34, "8" },
                { 35, "9" },
            #endregion
            };            
            
        }

        /// <summary>
        /// Move selected letter forward
        /// </summary>
        public void MoveLetterForward()
        {

            _selectedLetter++;
            if (_selectedLetter > 35)
            {
                _selectedLetter = 0;
            }
            SetSelectedLabel(true);
            LastOperation = DateTime.Now;
        }

        /// <summary>
        /// Move selected letter backward
        /// </summary>
        public void MoveLetterBackward()
        {
            _selectedLetter--;
            if (_selectedLetter < 0)
            {
                _selectedLetter = 35;
            }
            SetSelectedLabel(false);
            LastOperation = DateTime.Now;
        }

        /// <summary>
        /// Set new Selected letter
        /// </summary>
        /// <param name="forward">Inform if is a forward or backward move</param>
        private void SetSelectedLabel(bool forward)
        {
            Label lastLabel;
            Label selectedLabel = GetLabel(_selectedLetter);
            if (forward)
            {
                int lastLetterIndex = _selectedLetter - 1;
                if (lastLetterIndex < 0)
                {
                    lastLetterIndex = 35;
                }
                lastLabel = GetLabel(lastLetterIndex);
            }
            else
            {
                int lastLetterIndex = _selectedLetter + 1;
                if (lastLetterIndex > 35)
                {
                    lastLetterIndex = 0;
                }
                lastLabel = GetLabel(lastLetterIndex);
            }
            SetLabelColor(lastLabel, false);
            SetLabelColor(selectedLabel, true);
        }

        /// <summary>
        /// Get label name by Index
        /// </summary>
        /// <param name="index">index</param>
        /// <returns>Label name</returns>
        private string GetLabelName(int index)
        {
            string labelName = string.Format("_label{0}", _labelsDictionary[index]);
            return labelName;
        }

        /// <summary>
        /// Get label by index
        /// </summary>
        /// <param name="index">Index searched</param>
        /// <returns>Label who was searched</returns>
        private Label GetLabel(int index)
        {
            string labelName = GetLabelName(index);
            Label label = _pnlLetters.Controls.Find(labelName, false).FirstOrDefault() as Label;
            return label;
        }

        /// <summary>
        /// Set label color
        /// </summary>
        /// <param name="label">Label who will be changed</param>
        /// <param name="selected"> indicate if it's selected or not</param>
        private void SetLabelColor(Label label, bool selected)
        {
            if (selected)
            {
                label.ForeColor = System.Drawing.Color.OrangeRed;
                label.Font = new Font(label.Font.Name, 12, FontStyle.Bold);
            }
            else
            {
                label.ForeColor = System.Drawing.Color.WhiteSmoke;
                label.Font = new Font(label.Font.Name, 12, FontStyle.Regular);
            }
            
        }

        /// <summary>
        /// Set all labels to not selected
        /// </summary>
        private void ResetLabels()
        {
            for (int i = 0; i < 35; i++)
            {
                Label lb = GetLabel(i);
                SetLabelColor(lb, false);
            }
        }

        /// <summary>
        /// Starts panel and put it visible
        /// </summary>
        public void Start()
        {
            ResetLabels();
            _selectedLetter = -1;
            MoveLetterForward();
            _pnlLetters.Visible = true;
            _pnlLetters.Show();
            LastOperation = DateTime.Now;
        }

        /// <summary>
        /// Close panel 
        /// </summary>
        /// <returns>Selected letter when closing</returns>
        public string Close()
        {
            _pnlLetters.Visible = false;
            _pnlLetters.Hide();
            return SelectedLetter;
        }
    }
}

