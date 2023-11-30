using System.Numerics;

namespace WorkWithMatrix
{
    public class Matrix<T> where T : INumber<T>
    {
        private int _Rows;
        private int _Columns;
        private T[,] _Array;

        public int Rows { get => _Rows; }
        public int Columns { get => _Columns; }
        public T this[int row, int column]
        {
            get => _Array[row, column];
            set => _Array[row, column] = value;
        }

        private Matrix(int row, int column)
        {
            _Rows = row;
            _Columns = column;
            _Array = new T[_Rows, _Columns];
        }
        public Matrix(T[,] array)
        {
            _Rows = array.GetLength(0);
            _Columns = array.GetLength(1);
            _Array = new T[_Rows, _Columns];
            for (int r = 0; r < _Rows; r++)
                for (int c = 0; c < _Columns; c++)
                    _Array[r, c] = array[r, c];
        }

        public static Matrix<T> operator +(Matrix<T> leftMatrix, Matrix<T> rightMatrix)
        {
            if (leftMatrix._Rows == rightMatrix._Rows && leftMatrix._Columns == rightMatrix._Columns)
            {
                Matrix<T> result = new Matrix<T>(leftMatrix._Rows, leftMatrix._Columns);
                for (int r = 0; r < result._Rows; r++)
                    for (int c = 0; c < result._Columns; c++)
                        result._Array[r, c] = leftMatrix._Array[r, c] + rightMatrix._Array[r, c];
                return result;
            }
            else
                throw new NotImplementedException();
        }
        public static Matrix<T> operator -(Matrix<T> leftMatrix, Matrix<T> rightMatrix)
        {
            if (leftMatrix._Rows == rightMatrix._Rows && leftMatrix._Columns == rightMatrix._Columns)
            {
                Matrix<T> result = new Matrix<T>(leftMatrix._Rows, leftMatrix._Columns);
                for (int r = 0; r < result._Rows; r++)
                    for (int c = 0; c < result._Columns; c++)
                        result._Array[r, c] = leftMatrix._Array[r, c] - rightMatrix._Array[r, c];
                return result;
            }
            else
                throw new NotImplementedException();
        }
        public static Matrix<T> operator *(T multiplier, Matrix<T> matix)
        {
            Matrix<T> result = new Matrix<T>(matix._Rows, matix._Columns);
            for (int r = 0; r < result._Rows; r++)
                for (int c = 0; c < result._Columns; c++)
                    result._Array[r, c] = matix._Array[r, c] * multiplier;
            return result;
        }
        public static Matrix<T> operator *(Matrix<T> matix, T multiplier)
        {
            Matrix<T> result = new Matrix<T>(matix._Rows, matix._Columns);
            for (int r = 0; r < result._Rows; r++)
                for (int c = 0; c < result._Columns; c++)
                    result._Array[r, c] = matix._Array[r, c] * multiplier;
            return result;
        }
        public static Matrix<T> operator *(Matrix<T> leftMatrix, Matrix<T> rightMatrix)
        {
            if (leftMatrix._Columns == rightMatrix._Rows)
            {
                Matrix<T> result = new Matrix<T>(leftMatrix._Rows, rightMatrix._Columns);
                for (int rR = 0; rR < result._Rows; rR++)
                    for (int rC = 0; rC < result._Columns; rC++)
                        for (int l = 0; l < leftMatrix._Rows; l++)
                            result._Array[rR, rC] += leftMatrix._Array[rR, l] * rightMatrix._Array[l, rC];
                return result;
            }
            else
                throw new NotImplementedException();
        }

        public Matrix<T> GetInverse()
        {
            return T.One / GetDeterminant() * GetAdjointMatrix().GetTransposed();
        }
        public Matrix<T> GetTransposed()
        {
            Matrix<T> result = new Matrix<T>(_Columns, _Rows);
            for (int r = 0; r < result._Rows; r++)
                for (int c = 0; c < result._Columns; c++)
                    result._Array[r, c] = _Array[c, r];
            return result;
        }
        public Matrix<T> Pow(int exponent)
        {
            if (_Rows == _Columns)
            {
                Matrix<T> result = new Matrix<T>(_Array);
                for (int s = 0; s < exponent; s++)
                    result = this * result;
                return result;
            }
            else
                throw new NotImplementedException();
        }
        public Matrix<T> GetSubMatrix(int row, int column)
        {
            if (_Rows > 2 && _Columns > 2)
            {
                Matrix<T> result = new Matrix<T>(_Rows - 1, _Columns - 1);
                for (int r = 0; r < result._Rows; r++)
                {
                    int rI = r < row ? r : r + 1;
                    for (int c = 0; c < result._Columns; c++)
                        result._Array[r, c] = (c < column) ? _Array[rI, c] : _Array[rI, c + 1];
                }
                return result;
            }
            else
                throw new NotImplementedException();
        }
        public T GetMinor(int row, int column)
        {
            return GetSubMatrix(row, column).GetDeterminant();
        }
        public T GetAlgebraicComplement(int row, int column)
        {
            return (row + column) % 2 == 0 ? GetSubMatrix(row, column).GetDeterminant() : -T.One * GetSubMatrix(row, column).GetDeterminant();
        }
        public T GetDeterminant()
        {
            if (_Rows == _Columns)
            {
                if (_Rows == 1)
                    return _Array[0, 0];
                else if (_Rows == 2)
                    return _Array[0, 0] * _Array[1, 1] - _Array[0, 1] * _Array[1, 0];
                else if (_Rows == 3)
                    return _Array[0, 0] * _Array[1, 1] * _Array[2, 2] + _Array[0, 1] * _Array[1, 2] * _Array[2, 0] + _Array[0, 2] * _Array[1, 0] * _Array[2, 1] - _Array[0, 2] * _Array[1, 1] * _Array[2, 0] - _Array[0, 0] * _Array[1, 2] * _Array[2, 1] - _Array[0, 1] * _Array[1, 0] * _Array[2, 2];
                else
                {
                    Matrix<T> tMatrix = new Matrix<T>(_Array);
                    T result = T.Zero;
                    for (int r = 0; r < tMatrix._Rows; r++)
                        result += _Array[r, 0] * GetAlgebraicComplement(r, 0);
                    return result;
                }
            }
            else
                throw new NotImplementedException();
        }
        public Matrix<T> GetAdjointMatrix()
        {
            Matrix<T> result = new Matrix<T>(_Rows, _Columns);
            for (int r = 0; r < _Rows; r++)
                for (int c = 0; c < _Columns; c++)
                    result._Array[r, c] = GetAlgebraicComplement(r, c);
            return result;
        }
        public bool IsSquare()
        {
            return (_Rows == _Columns) ? true : false;
        }
        public void RowsSwap(int r1, int r2)
        {
            for (int c = 0; c < _Columns; c++)
            {
                T value = _Array[r2, c];
                _Array[r2, c] = _Array[r1, c];
                _Array[r1, c] = value;
            }
        }
        public int GetRang()
        {
            Matrix<T> tMatrix = new Matrix<T>(_Array);
            for (int c = 0; c < tMatrix._Columns; c++)
                if (tMatrix._Rows > c)
                {
                    if (tMatrix._Array[c, c] == T.Zero)
                        for (int rI = c + 1; rI < tMatrix._Rows; rI++)
                            if (tMatrix._Array[rI, c] != T.Zero)
                                RowsSwap(c, rI);
                    if (tMatrix._Array[c, c] != T.Zero)
                        for (int r = c + 1; r < tMatrix._Rows; r++)
                            if (tMatrix._Array[r, c] != T.Zero)
                            {
                                T divisor = tMatrix._Array[r, c] / tMatrix._Array[c, c];
                                for (int p = 0; p < tMatrix._Columns; p++)
                                    tMatrix._Array[r, p] = tMatrix._Array[r, p] - tMatrix._Array[c, p] * divisor;
                            }
                }
            int result = 0;
            for (int r = 0; r < tMatrix._Rows; r++)
                for (int c = 1; c < tMatrix._Columns; c++)
                    if (tMatrix._Array[r, c] != T.Zero)
                    {
                        result += 1;
                        break;
                    }
            return result;
        }
    }
}