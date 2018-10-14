import java.util.Random;
import java.util.concurrent.*;
import java.util.ArrayList;

public class Matrix {
    public int[][] data;
    public int size;

    

    public Matrix Multiply01(Matrix b)
    {
        int size = this.size;
        Matrix m = new Matrix(size, 0);
        for(int i = 0; i < size; i++)
        {
            for(int j = 0; j < size; j++)
            {
                for( int k = 0; k < size; k++)
                {
                    m.data[i][j] += this.data[i][k] * b.data[k][j];
                }
            }
        }
        return m;
    }

    public Matrix Multiply02(Matrix b)
    {
        int size = this.size;
        Matrix m = new Matrix(size, 0);
        for (int i = 0; i < size; i++)
        {
            for (int k = 0; k < size; k++)
            {
                for (int j = 0; j < size; j++)
                {
                    m.data[i][j] += this.data[i][k] * b.data[k][j];
                }
            }

        }
        return m;
    }
    
    public Matrix MultiplyThread01(Matrix b, int threadCount){
    	int size = this.size;
        Matrix m = new Matrix(size, 0);
        int rowPerThread;
        int lastThreadRows;
        if (b.size % threadCount != 0)
        {
            rowPerThread = b.size / (threadCount - 1);
            lastThreadRows = b.size % (threadCount - 1);
        }
        else
        {
            rowPerThread = b.size / threadCount;
            lastThreadRows = 0;
        }

        CountDownLatch latch = new CountDownLatch(threadCount);
        
        ArrayList<MultiplierThread> list = new ArrayList<MultiplierThread>();
        ArrayList<Thread> threadList = new ArrayList<Thread>();
        
        for (int i = 0; i < threadCount - 1; i++)
        {
        	list.add(i, new MultiplierThread(this, b, m, i * rowPerThread, rowPerThread, latch));
        	threadList.add(i, new Thread(list.get(i)));
        	threadList.get(i).start();
        }
        
        list.add(threadCount - 1, new MultiplierThread(this, b, m, (threadCount - 1) * rowPerThread, lastThreadRows, latch));
        threadList.add(new Thread(list.get(threadCount-1)));
        threadList.get(threadCount-1).start();
        
        
        
        try {
			latch.await();
		} catch (InterruptedException e) {
			System.out.println("Interrupted!");
		}
    	
    	return m;
    }
    
    
    public Matrix MultiplyThread02(Matrix b, int threadCount){
    	int size = this.size;
        Matrix m = new Matrix(size, 0);
        int rowPerThread;
        int lastThreadRows;
        
        ExecutorService executor = Executors.newFixedThreadPool(threadCount);
        
        if (b.size % threadCount != 0)
        {
            rowPerThread = b.size / (threadCount - 1);
            lastThreadRows = b.size % (threadCount - 1);
        }
        else
        {
            rowPerThread = b.size / threadCount;
            lastThreadRows = 0;
        }

        
        
        ArrayList<MultiplierThreadES> list = new ArrayList<MultiplierThreadES>();
        
        
        for (int i = 0; i < threadCount - 1; i++)
        {
        	list.add(i, new MultiplierThreadES(this, b, m, i * rowPerThread, rowPerThread));
        	executor.execute(list.get(i));
        }
        
        list.add(threadCount - 1, new MultiplierThreadES(this, b, m, (threadCount - 1) * rowPerThread, lastThreadRows));
        executor.execute(list.get(threadCount-1));
        
        executor.shutdown();
        try {
			executor.awaitTermination(10, TimeUnit.SECONDS);
		} catch (InterruptedException e) {
			
		}
    	
    	return m;
    }


    

    public Matrix(int size)
    {
        this.size = size;
        data = new int[size][size];
        Random rnd =new Random();
        for (int i = 0; i < size; i++)
        {
            for(int j = 0; j < size; j++)
            {
                data[i][j] = rnd.nextInt();
            }
        }
    }

    public Matrix(int size, int number)
    {
        this.size = size;
        data = new int[size][size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                data[i][j] = number;
            }
        }
    }

}


