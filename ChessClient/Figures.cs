using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ChessClient
{
    class Figures
    {
        private readonly string[] _codStrings;
        public bool Color {  get; private set; }

        public int Name { get; private set; }

        public int Position { set; get; }

        public int Index { set; get; }

        public  Figures( int name ,int position)
        {
            _codStrings = new[] { "&#9814;", "&#9816;", "&#9815;", "&#9813;", "&#9812;", "&#9815;", "&#9816;", "&#9814;",   // белые фигуры
                                 "&#9817;", "&#9817;", "&#9817;", "&#9817;", "&#9817;", "&#9817;", "&#9817;", "&#9817;",   // белые пешки
                                 "&#9823;", "&#9823;", "&#9823;", "&#9823;", "&#9823;", "&#9823;", "&#9823;", "&#9823;",   // черные пешки
                                 "&#9820;", "&#9822;", "&#9821;", "&#9819;", "&#9818;", "&#9821;","&#9822;", "&#9820;"};   // черные фигуры

            Name = name;
            Color = Name < 16 ?  true :  false;
            Position = position;
            
        }

        public string GetCod()
        {
            return HttpUtility.HtmlDecode(_codStrings[Name]);
        }
    }

}
