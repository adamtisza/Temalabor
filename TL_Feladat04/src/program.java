import java.util.Random;
import java.util.concurrent.*;

public class program {
	public static void main(String[] args) {
		
		int size = Integer.parseInt(args[0]);
        int[] block = generateBlock(size); 
        long lStartTime = 0;
        long lEndTime = 0;
        final int minBlockSize = 1000;
        
        
        switch (args[1])
        {
            case "1":
            	lStartTime = System.nanoTime();
                quickSort(block, 0, size - 1); 
                break;
            case "2":
            	ExecutorService es = Executors.newCachedThreadPool();
            	lStartTime = System.nanoTime();
                mergeSort(block, 0, size - 1, minBlockSize, es);
                break;
            default:
                break;
        }
        lEndTime = System.nanoTime();
        long elapsed = (lEndTime - lStartTime)/1000;
        
        System.out.println(elapsed + " us");
        
	}

	static int[] generateBlock(int size) {
		int[] block = new int[size];
		Random rnd = new Random();

		for (int i = 0; i < size; i++) {
			block[i] = rnd.nextInt();
		}
		return block;

	}

	static int partition(int[] arr, int left, int right) {
		int i = left, j = right;
		int tmp;
		int pivot = arr[(left + right) / 2];

		while (i <= j) {

			while (arr[i] < pivot)
				i++;
			while (arr[j] > pivot)
				j--;
			if (i <= j) {
				tmp = arr[i];
				arr[i] = arr[j];
				arr[j] = tmp;
				i++;
				j--;
			}
		}
		;
		return i;
	}

	static void quickSort(int[] arr, int left, int right) {
		int index = partition(arr, left, right);
		if (left < index - 1)
			quickSort(arr, left, index - 1);
		if (index < right)
			quickSort(arr, index, right);
	}

	static void mergeSort(int[] array, int left, int right, int minArraySize, ExecutorService es) {
		if (right <= left)
			return;
		int mid = ((right + left) / 2);
		if (right - mid >= minArraySize) {
			Runnable task1 = () -> {
				mergeSort(array, left, mid, minArraySize, es);
			};
			Runnable task2 = () -> {
				mergeSort(array, mid + 1, right, minArraySize, es);
			};

			try {
				Future f1 = es.submit(task1);
				Future f2 = es.submit(task2);
				f1.get();
				f2.get();
			} catch (ExecutionException ee) {
				System.out.println("Execution failed!");
			} catch (InterruptedException ie) {
				System.out.println("Task interrupted!");
			}
		} else {
			mergeSort(array, left, mid, minArraySize, es);
			mergeSort(array, mid + 1, right, minArraySize, es);
		}
		merge(array, left, mid, right);
	}

	

	static void merge(int arr[], int l, int m, int r) 
	{ 
	    int i, j, k; 
	    int n1 = m - l + 1; 
	    int n2 =  r - m; 
	  

	    int[] L = new int[n1];
	    int[] R = new int[n2];
	  

	    for (i = 0; i < n1; i++) 
	        L[i] = arr[l + i]; 
	    for (j = 0; j < n2; j++) 
	        R[j] = arr[m + 1+ j]; 
	  
	    i = 0;
	    j = 0;
	    k = l; 
	    while (i < n1 && j < n2) 
	    { 
	        if (L[i] <= R[j]) 
	        { 
	            arr[k] = L[i]; 
	            i++; 
	        } 
	        else
	        { 
	            arr[k] = R[j]; 
	            j++; 
	        } 
	        k++; 
	    } 
	  

	    while (i < n1) 
	    { 
	        arr[k] = L[i]; 
	        i++; 
	        k++; 
	    } 
	  

	    while (j < n2) 
	    { 
	        arr[k] = R[j]; 
	        j++; 
	        k++; 
	    } 
	} 
	  
	
	
	

}
