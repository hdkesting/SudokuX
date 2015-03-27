using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace SudokuX.Controls
{
    public partial class GridField : System.Windows.Forms.Control
    {
        private readonly bool[] _possibles;
        private int? _givenValue;
        private int? _userValue;

        private readonly List<List<GridField>> _groups = new List<List<GridField>>();
        private Font _bigFont;
        private Font _smallFont;
        private Brush _smallBrush;
        private Brush _givenBrush;
        private Brush _userBrush;

        /// <summary>
        /// Initializes a new instance of the <see cref="GridField" /> class.
        /// </summary>
        /// <param name="size">The pixel size (width and height) of the field.</param>
        /// <param name="edgeX">The block size in x direction.</param>
        /// <param name="edgeY">The block size in y direction.</param>
        public GridField(int size, int edgeX, int edgeY)
        {
            InitializeComponent();

            Width = Height = size;
            GridEdgeX = edgeX;
            GridEdgeY = edgeY;
            Margin = new System.Windows.Forms.Padding(1);
            _possibles = new bool[GridSize];

            InitializeControl();
        }

        public GridField(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            InitializeControl();
        }

        private void InitializeControl()
        {
            for (int i = 0; i < GridSize; i++)
                _possibles[i] = true;
        }

        private int GridEdgeX { get; set; }
        private int GridEdgeY { get; set; }
        public int GridSize { get { return GridEdgeX * GridEdgeY; } }

        public int? Value
        {
            get { return _userValue ?? _givenValue; }
        }

        public IEnumerable<List<GridField>> ContainingGroups { get { return _groups.AsReadOnly(); } }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);

            float smallsize = ((Width - 4) / (float)GridEdgeX);
            _bigFont = _bigFont ?? new Font("Segoe UI", Width - 4.0f, GraphicsUnit.Pixel);
            _smallFont = _smallFont ?? new Font("Segoe UI", smallsize * .75f, GraphicsUnit.Pixel);
            _smallBrush = _smallBrush ?? new SolidBrush(Color.FromArgb(16, 16, 16));
            _givenBrush = _givenBrush ?? new SolidBrush(Color.FromArgb(16, 16, 16));
            _userBrush = _userBrush ?? new SolidBrush(Color.FromArgb(16, 16, 164));

            var allRect = new Rectangle(0, 0, Width, Height);
            // achtergrond
            using (var back = new SolidBrush(this.BackColor))
            {
                e.Graphics.FillRectangle(back, allRect);
            }

            // border (wit)
            using (var border = new Pen(Color.FromKnownColor(System.Drawing.KnownColor.AntiqueWhite), 4.0f))
            {
                e.Graphics.DrawRectangle(border, allRect);
            }

            // mogelijkheden of enkel getal
            if (_givenValue.HasValue || _userValue.HasValue)
            {
                int value = _givenValue ?? _userValue.Value;
                // groot, enkel getal
                e.Graphics.DrawString(GetChar(value), _bigFont, _givenValue.HasValue ? _givenBrush : _userBrush, 1f, -5f);
            }
            else
            {
                // n kleine getallen (voor zover "possible")
                for (var x = 0; x < GridEdgeX; x++)
                    for (var y = 0; y < GridEdgeY; y++)
                    {
                        var c = y * GridEdgeX + x;
                        if (_possibles[c])
                        {
                            e.Graphics.DrawString(GetChar(c), _smallFont, _smallBrush, smallsize * x + 1.5f, smallsize * y + 1.5f);
                        }
                    }
            }
        }

        public void SetValue(int value, bool user)
        {
            if (user)
                _userValue = value;
            else
                _givenValue = value;

            Invalidate();
        }

        private string GetChar(int value)
        {
            if (GridSize < 10)
                return "123456789"[value].ToString();

            return "0123456789ABCDEF"[value].ToString();
        }

        public void AddGroup(List<GridField> grp)
        {
            _groups.Add(grp);
        }

        public void RemovePossibility(int value)
        {
            _possibles[value] = false;
            Invalidate();
        }

        public bool IsPossible(int value)
        {
            return _possibles[value];
        }
    }
}
