using System;
using System.Collections.Generic;

namespace Task_1
{
    class MatrixSizeException : Exception
    {
        public MatrixSizeException(): base(){}
        public MatrixSizeException(string str): base(str){}
        public MatrixSizeException(string str, Exception inner): base(str, inner){}
        public override string ToString()
        {
            return Message;
        } 
    }
    class Matrix:ICloneable
    {
        private int[,] _array;
        public Matrix()
        {
            Random random= new Random();
            Array=new int[random.Next(10),random.Next(10)];
            for (int i=0; i<_array.GetLength(0); i++)
            {
                for (int j=0; j<_array.GetLength(1); j++)
                {
                    this[i,j]=random.Next(10);           
                }
            }
        }

        public Matrix(int rows, int cols)
        {
          Array=new int[rows,cols];
        }
        
        public Matrix (int rows, int cols, int[] array)
        {
            if (rows*cols!=array.Length)
            {
                throw new MatrixSizeException("Provided sizes and he size of the array do not match.");
            }

            Array= new int[rows, cols];
            for(int i=0; i<array.Length;i++)
            {
                this[i/cols,i%cols]=array[i];
            }
        }

        public Matrix (int[,] array)
        {
            Array=(int[,]) array.Clone();
        }


        public int this[int row, int col]
        {
            get
            {
                if ((row>Rows-1)||(col>Columns-1)||
                (row<0)||(col<0))
                {
                    throw new MatrixSizeException("Index out of range");
                }
                return Array[row,col];

            }
            private set
            {
                Array[row,col]=value;
            }
        }
        private int[,] Array
        {
            get =>_array;
            set=> _array=value;
        }

        public int Columns
        {
            get =>Array.GetLength(1);
        }

        public int Rows
        {
            get =>Array.GetLength(0);
        }


        public static Matrix operator +(Matrix a, Matrix b)
        {
            if (a.Rows!=b.Rows||
            a.Columns!=b.Columns)
            {
                throw new MatrixSizeException("Matrixes have different sizes.");
            }

            Matrix c=new Matrix(a.Rows, a.Columns);
            
            for(int i=0; i<a.Array.GetLength(0); i++)
            {
                for(int j=0; j<a.Array.GetLength(0); j++)
                {
                    c[i,j]=a[i,j]+b[i,j];
                }
            }
            return c;

        }

        public static Matrix operator -(Matrix a, Matrix b)
        {

            return a+(b*(-1));

        }


        public static Matrix operator *(Matrix a, int num)
        {
            Matrix res= new Matrix(a.Array);

            for (int i=0; i<res.Rows; i++)
            {
                for (int j=0; j<res.Columns; j++)
                {
                    res[i,j]*=num;
                }
            }
            return res;
        }

        public static Matrix operator *(int num, Matrix a)
        {
            return a*num;
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (a.Columns!=b.Rows) throw new MatrixSizeException("Matrix sizes do not match");

            Matrix res= new Matrix (a.Rows, b.Columns);

            for (int i=0; i<a.Rows; i++)
            {
                for (int j=0; j<b.Columns; j++)
                {
                    int sum=0;
                    for (int l=0; l<a.Columns; l++)
                    {
                        sum+=a[i,l]*b[l,j];
                    }
                    res[i,j]=sum;
                }
            }
            return res;
        }

        public static bool operator <(Matrix a, Matrix b)
        {
            return (a.Det()<b.Det());
        }

        public static bool operator >(Matrix a, Matrix b)
        {
            return !(a<b);
        }

        public static bool operator ==(Matrix a, Matrix b)
        {
            return (a.Det()==b.Det());
        }

        public static bool operator !=(Matrix a, Matrix b)
        {
            return !(a.Det()==b.Det());
        }
        public int Det()
        {
            if (Rows!=Columns) throw new MatrixSizeException("The Matrix is not square");
            return Determinant((int[,])Array.Clone());
        }
        private int Determinant(int[,] array )
        {
            int d = 0;
            if (array.Length == 4) 
            {
                d=  array[0,0] * array[1,1] - array[1,0] * array[0,1];
            }
            else
            {  
                for(int c = 0; c < array.GetLength(1); c++)
                {
                    d+=  (((int) Math.Pow(-1 ,c)) * array[0,c] * Determinant(Submatrix(array, c)));
                    
                }
            }
            return d;
        }

        public int[,] Submatrix(int[,] mat, int n)
        {
            int[,] sub=new int[mat.GetLength(0)-1,mat.GetLength(0)-1];
            
            for (int i = 0, curI=1; i < mat.GetLength(0) - 1; i++, curI++)
            {
                for (int j = 0, curJ=0; j < mat.GetLength(0) - 1; j++, curJ++)
                {
                    if (j == n) curJ++;

                    sub[i, j] = mat[curI, curJ];
                }   
            }

            return sub;
        }
        public object Clone()
        {
            return (object) new Matrix((int[,]) this.Array.Clone());
        }


    }
    class Program
    {
        static void Main(string[] args)
        {
            /* 
            Matrix a=new Matrix( new int[,]{{1, 2} ,{3, 4}, {5, 6}});
            Matrix b=new Matrix(new int[,]{{1, 2 ,1}, {3, 2, 3}});

            Matrix c=a*b;
            for (int i=0; i<c.Rows; i++)
            {
                for (int j=0; j<c.Columns; j++)
                {
                    Console.WriteLine(c[i,j]);
                }
            }
            Matrix d= (Matrix)c.Clone()*3;
            Console.WriteLine("######################");
            for (int i=0; i<d.Rows; i++)
            {
                for (int j=0; j<d.Columns; j++)
                {
                    Console.WriteLine(d[i,j]);
                }
            }

            d=c-c;
            Console.WriteLine("######################");
            for (int i=0; i<d.Rows; i++)
            {
                for (int j=0; j<d.Columns; j++)
                {
                    Console.WriteLine(d[i,j]);
                }
            }
            Matrix e= new Matrix(new int[,]{{1,13,8,2},{9,0,2,1},{8,4,2,7},{5,2,6,7}});
            Matrix f= new Matrix(new int[,]{{1,13,8},{9,0,2},{8,4,2}});
            Console.WriteLine(e.Det());
            Console.WriteLine(f.Det());
            Console.WriteLine(e>f);
            Polynomial w=new Polynomial();*/

            Polynomial expr1= new Polynomial(1,0,2,0,3);
            Console.WriteLine(expr1);
            Polynomial expr2= new Polynomial(new SortedDictionary<int,int> {{6,3},{5,-2}});
            Console.WriteLine(expr2);
            Console.WriteLine(expr1.Power(3));
        }
    }
}
