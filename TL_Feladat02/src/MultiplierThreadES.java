
public class MultiplierThreadES implements Runnable{

	Matrix a, b, m;
	int size;
	int startRow, rowCount;
	
	MultiplierThreadES(Matrix a, Matrix b, Matrix m, int startRow, int rowCount){
		this.a = a;
		this.b = b;
		this.m = m;
		this.size = a.size;
		this.startRow = startRow;
		this.rowCount = rowCount;
		
		
	}

	public void run() {
		int size = this.size;

		for (int i = 0; i < size; i++)
		{
			for (int k = startRow; k < startRow + rowCount; k++)
			{
				for (int j = 0; j < size; j++)
				{
                    m.data[i][j] += a.data[i][k] * b.data[k][j];
				}
			}
		}
	}
}
