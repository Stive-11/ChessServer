using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ChessClient
{
    class Table
    {
        const int FigureSize=60;
        private Grid TableGrid;
        private UniformGrid TableUniformgrid;
        private int[] arTable = new int[64];
        private Figures[] figureses = new Figures[32];

        public Table(ref Grid grid, ref UniformGrid uniformGrid)
        {
            TableGrid = grid;
            TableUniformgrid = uniformGrid;
            CreateTable();
            CreateFigures();

        }

        public int StringToNumber(string s)
        {
            s=s.ToLower();
            var n = ((byte)s[0] - (byte)'a') + (int.Parse(s[1].ToString())-1)*8;
            return n;
        }

        private void CreateTable()   // создание доски
        {
            Rectangle ar = new Rectangle();
            for (int i = 0; i < 64; i++)
            {
                ar = new Rectangle();
                ar.Fill = ((i % 8) + (i / 8)) % 2 == 1 ? Brushes.Black : Brushes.White;
                TableUniformgrid.Children.Add(ar);
            }
        }

        private void CreateFigures() // создание фигур
        {
            for (int i = 0; i < 32; i++)
            {
                var pos = i < 16 ? i : i + 32;
                figureses[i] = new Figures(i,pos);
                arTable[pos] = i;               // расстановка номеров фигур
                arTable[i + 16] = 100;          // пустые клетки
                // размещение самих фигур в окне программы
                UserControl uc = new UserControl();
                uc.Content = figureses[i].GetCod();
                uc.Height = uc.Width =FigureSize;
                uc.VerticalAlignment = VerticalAlignment.Top;
                uc.HorizontalAlignment = HorizontalAlignment.Left;
                uc.VerticalContentAlignment = VerticalAlignment.Center;
                uc.HorizontalContentAlignment = HorizontalAlignment.Center;
                uc.FontSize = FigureSize - 15;
                uc.Name = "F" + i.ToString();
                uc.Margin = new Thickness(GetX(pos)*FigureSize , GetY(pos)*FigureSize,0,0);
                TableGrid.Children.Add(uc);
            }
        }

        private int GetX(int pos)
        {
            return pos%8;
        }  // получение координаты х

        private int GetY(int pos)
        {
            return 7 - pos / 8;
        } // получение координаты у


        public bool MoveFigure(string stBegin, string stEnd)   // перемещение фигуры
        {
            int nBegin = StringToNumber(stBegin);
            int nEnd = StringToNumber(stEnd);
            if (!CanMove (nBegin,nEnd)) return false;

            else
            {
                var fNumber = arTable[nBegin];
                arTable[nBegin] = 100;

                if (arTable[nEnd] != 100)               // замена фигуры
                {
                    foreach (var ch in TableGrid.Children)
                    {
                        if (((UserControl) ch).Name == "F" + arTable[nEnd].ToString())
                        {
                            TableGrid.Children.Remove((UIElement)ch);
                            break;
                        }
                    }

                    figureses[arTable[nEnd]].Position = 1000;
                }

                arTable[nEnd] = fNumber;
                figureses[fNumber].Position = nEnd;
                foreach (var ch in TableGrid.Children)
                {
                    if (((UserControl)ch).Name == "F" + arTable[nEnd].ToString())
                    {
                        ((UserControl)ch).Margin = new Thickness(GetX(nEnd) * FigureSize, GetY(nEnd) * FigureSize, 0, 0);
                        break;
                    }
                }
                return true;
            }
        }

        private bool CanMove(int nBegin, int nEnd)
        {
            if (arTable[nBegin] == 100) return false;
            if (arTable[nEnd] == 100) return true;
            if (figureses[arTable[nBegin]].Color == figureses[arTable[nEnd]].Color) return false;
           else
           {
               return true;
           }
        }

        public string GetFigure(string s)
        {
            return figureses[arTable[StringToNumber(s)]].GetCod();
        }

        public void GameOver()
        {
            
        }
    }
}
