# Introduction  
For this project, a simple 3-layer 2D-CNN model was used. A custom dataset creation system was developed, along with scalable image preprocessing.

# Dataset Creation  
Reference PNG images of symbols were used as the base. These images were processed with slight modifications, including rotation, color adjustments, and brightness changes. This process was repeated until each class contained 50 sample images.

# Preprocessing  
The original images were converted to grayscale, resized to 64x64 pixels, and transformed into vectors (all images were in PNG format initially).

# Model Architecture  
As mentioned earlier, the model consists of three 2D-CNN layers with batch normalization. L2 regularization and dropout were applied (with rates of 0.3 for the first layer, 0.4 for the second, and 0.5 for the third). The output layer is a dense layer with softmax activation, providing smoother results and clearer confidence scores for each class prediction.

# Model Conversion  
After training, the model was converted to ONNX format for seamless integration into C#. The same preprocessing steps were applied to the images in the C# environment.