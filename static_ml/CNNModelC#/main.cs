using ONNXPredictor;

public class Program
{
    public static void Main()
    {
        // create a predictor
        using var predictor = new ONNXPredictor("CNN_model.onnx");
        // get most confiednt class and confidence itself
        var (predictedClass, confidence) = predictor.PredictWithConfidence("air3.png");
        
        Console.WriteLine(predictor.GetClassName(predictedClass));
        // get all class probabilities( confidence to each class
        var allProbs = predictor.GetAllClassProbabilities("air3.png");
        
        foreach (var kvp in allProbs.OrderByDescending(x => x.Value))
        {
            Console.WriteLine($"{kvp.Key}: {kvp.Value:P2}");
        }
    }
}