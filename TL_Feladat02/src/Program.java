


public class Program {
	public static void main(String[] args)
    {
		int size = Integer.parseInt(args[0]);
        Matrix a = new Matrix(size);
        Matrix b = new Matrix(size);
        long lStartTime = 0;
        long lEndTime = 0;
        switch (args[1])
        {
            case "1":
            	lStartTime = System.nanoTime();
                a.Multiply01(b);
                break;
            case "2":
            	lStartTime = System.nanoTime();
                a.Multiply02(b);
                break;
            case "3":
            	lStartTime = System.nanoTime();
                a.MultiplyThread01(b, Integer.parseInt(args[2]));
                break;
            case "4":
            	lStartTime = System.nanoTime();
                a.MultiplyThread02(b, Integer.parseInt(args[2]));
                break;
            default:
                break;
        }
        
        lEndTime = System.nanoTime();
        long elapsed = (lEndTime - lStartTime)/1000;
        
        System.out.println(elapsed + " us");
        
		
        
    }
}

