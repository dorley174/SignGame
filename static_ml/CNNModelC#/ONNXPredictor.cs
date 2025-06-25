using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;


namespace ONNXPredictor
{
        
    public class ModelInput
    {
        [VectorType(1, 64, 64)]     
        [ColumnName("args_0")]   
        public float[] ImageData { get; set; }
    }

    public class ModelOutput
    {
        [ColumnName("output")]  
        public float[] Predictions { get; set; }
    }

    public class ONNXPredictor : IDisposable
    {
        private readonly MLContext _mlContext;
        private readonly PredictionEngine<ModelInput, ModelOutput> _predictionEngine;
        private readonly float _confidenceThreshold;
        Dictionary<int, String> _class_cnt;

        public ONNXPredictor(string onnxModelPath, float confidenceThreshold = 0.7f)
        {
            _confidenceThreshold = confidenceThreshold;
            _mlContext = new MLContext();

            var pipeline = _mlContext.Transforms.ApplyOnnxModel(
                modelFile: onnxModelPath,
                outputColumnName: "output",
                inputColumnName: "args_0");

            var emptyData = _mlContext.Data.LoadFromEnumerable(new List<ModelInput>());

            var model = pipeline.Fit(emptyData);

            _predictionEngine = _mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(model);

            string targetDirectory = "static_ml\etalon";
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            _class_cnt = new Dictionary<int, String>();
            int cnt = 0;
            foreach (string fileName in fileEntries)
                _class_cnt.Add(cnt++, fileName);
        }

        public static float[] ProcessImageToArray(string imgPath)
        {
            float[,,] processed = ProcessImage(imgPath);

            float[] flatArray = new float[1 * 64 * 64];
            int index = 0;

            for (int y = 0; y < 64; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    flatArray[index++] = processed[y, x, 0];
                }
            }

            return flatArray;
        }

        public static float[,,] ProcessImage(string imgPath)
        {
            using ( var image = Image.Load<Rgb24>(imgPath))
            {
                image.Mutate(x => x
                    .Grayscale()
                    .Resize(64, 64));

                float[,,] imgArray = new float[64, 64, 1];

                for (int y = 0; y < 64; y++)
                {
                    for (int x = 0; x < 64; x++)
                    {
                        var pixel = image[x, y];
                        float grayValue = (pixel.R * 0.299f + pixel.G * 0.587f + pixel.B * 0.114f) / 255f;
                        imgArray[y, x, 0] = grayValue;
                    }
                }

                return imgArray;
            }
        }
        // return class and confidence of prediction
        // return (-1, 0f) of model not confident of prediction
        public (int PredictedClass, float Confidence) PredictWithConfidence(string imagePath)
        {
            var input = new ModelInput { ImageData = ProcessImageToArray(imagePath) };
            var prediction = _predictionEngine.Predict(input);

            var maxConfidence = prediction.Predictions.Max();
            var predictedClass = Array.IndexOf(prediction.Predictions, maxConfidence);

            if (maxConfidence < _confidenceThreshold)
            {
                return (-1, 0f); 
            }

            return (predictedClass, maxConfidence);
        }

        public Dictionary<string, float> GetAllClassProbabilities(string imagePath)
        {
            var input = new ModelInput { ImageData = ProcessImageToArray(imagePath) };
            var prediction = _predictionEngine.Predict(input);

            var classProbabilities = new Dictionary<string, float>();
            for (int i = 0; i < prediction.Predictions.Length; i++)
            {
                classProbabilities.Add(GetClassName(i), prediction.Predictions[i]);
            }

            return classProbabilities;
        }
        
         

        public string GetClassName(int classIndex)
        {
            return _class_cnt[classIndex];
        }

        public void Dispose()
        {
            _predictionEngine?.Dispose();
        }
    }

}