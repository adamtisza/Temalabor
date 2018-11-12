import java.util.Random;
import java.util.concurrent.Executors;
import java.util.concurrent.TimeUnit;
import java.util.concurrent.ExecutorService;
import java.lang.Runnable;

public class program {
	public static void main(String[] args) {
		
		int size = Integer.parseInt(args[0]);
        int[] block = generateBlock(size); 
        long lStartTime = 0;
        long lEndTime = 0;
        int minBlockSize = 100;
        
        
        switch (args[1])
        {
            case "1":
            	lStartTime = System.nanoTime();
                quickSort(block, 0, size - 1); 
                break;
            case "2":
            	lStartTime = System.nanoTime();
                mergeSortInPlace(block, 0, size - 1, minBlockSize);
                break;
            default:
                break;
        }
        lEndTime = System.nanoTime();
        long elapsed = (lEndTime - lStartTime)/1000;
        
        System.out.println(elapsed + " us");
        

        
		
	}
	
	static int[] generateBlock(int size)
    {
        int[] block = new int[size];
        Random rnd = new Random();

        for(int i = 0; i < size; i++)
        {
            block[i] = rnd.nextInt();
        }
        return block;

    }
	
	static int partition(int[] arr, int left, int right)
    {
        int i = left, j = right;
        int tmp;
        int pivot = arr[(left + right) / 2];

        while (i <= j)
        {

            while (arr[i] < pivot)
                i++;
            while (arr[j] > pivot)
                j--;
            if (i <= j)
            {
                tmp = arr[i];
                arr[i] = arr[j];
                arr[j] = tmp;
                i++;
                j--;
            }
        };
        return i;
    }

    static void quickSort(int[] arr, int left, int right)
    {
        int index = partition(arr, left, right);
        if (left < index - 1)
            quickSort(arr, left, index - 1);
        if (index < right)
            quickSort(arr, index, right);
    }
    
    static void mergeSortInPlace( int[] array, int left, int right , int minArraySize)
    {
        if ( right <= left ) return;
        int mid = (( right + left ) / 2 );
        if(right-mid >= minArraySize) {
        	ExecutorService es = Executors.newFixedThreadPool(2);
        	
        	Runnable task1 = () -> {
        		mergeSortInPlace( array, left,     mid, minArraySize);
        	};
        	Runnable task2 = () -> {
        		mergeSortInPlace( array, mid + 1, right, minArraySize);
        	};
        	

        		es.execute(task1);
        		es.execute(task2);

        	
        	
        	es.shutdown();
        	try {
    			es.awaitTermination(10, TimeUnit.SECONDS);
    		} catch (InterruptedException e) {
    			
    		}
        }
        else {
        	mergeSortInPlace( array, left,     mid, minArraySize);
        	mergeSortInPlace( array, mid + 1, right, minArraySize);
        }
        inPlaceMerge( array, left, mid, right );
    }
    
    private static void inPlaceMerge(int[] array, int left, int mid, int right) {
        int secondArrayStart = mid+1;
        int prevPlaced = mid+1;
        int q = mid+1;
        while(left < mid+1 && q <= right){
            boolean swapped = false;
            if(array[left] > array[q]) {
                swap(array, left, q);
                swapped = true;
            }   
            if(q != secondArrayStart && array[left] > array[secondArrayStart]) {
                swap(array, left, secondArrayStart);
                swapped = true;
            }
            
            if(swapped && secondArrayStart+1 <= right && array[secondArrayStart+1] < array[secondArrayStart]) {
                prevPlaced = placeInOrder(array, secondArrayStart, prevPlaced);
            }
            left++;
            if(q < right) {     //q+1 <= r) {
                q++;
            }
        }
    }
    
    static int placeInOrder(int[] array, int secondArrayStart, int prevPlaced) {
        int i = secondArrayStart;
        for(; i < array.length; i++) {
            //Simply swap till the prevPlaced position
            if(secondArrayStart < prevPlaced) {
                swap(array, secondArrayStart, secondArrayStart+1);
                secondArrayStart++;
                continue;
            }
            if(array[i] < array[secondArrayStart]) {
                swap(array, i, secondArrayStart);
                secondArrayStart++;
            } else if(i != secondArrayStart && array[i] > array[secondArrayStart]){
                break;
            }
        }
        return secondArrayStart;
    }

    private static void swap(int[] array, int m, int n){
        int temp = array[m];
        array[m] = array[n];
        array[n] = temp;
    }
    
    
}
