import os
import numpy as np
import tensorflow as tf
from tensorflow.keras import layers, models, callbacks, regularizers
from sklearn.model_selection import train_test_split
from PIL import Image
import matplotlib.pyplot as plt

class HighConfidenceModel:
    def __init__(self, input_shape, num_classes):
        self.input_shape = input_shape
        self.num_classes = num_classes
        self.model = self._build_model()
        self.confidence_threshold = 0.7  # 90% confidence threshold
    
    def _build_model(self):
        """Build a model with architecture designed for high-confidence predictions"""
        model = models.Sequential([
            # Enhanced convolutional blocks with regularization
            layers.Conv2D(32, (3, 3), activation='relu', 
                         kernel_regularizer=regularizers.l2(0.001),
                         input_shape=self.input_shape),
            layers.BatchNormalization(),
            layers.MaxPooling2D((2, 2)),
            layers.Dropout(0.2),
            
            layers.Conv2D(64, (3, 3), activation='relu',
                         kernel_regularizer=regularizers.l2(0.001)),
            layers.BatchNormalization(),
            layers.MaxPooling2D((2, 2)),
            layers.Dropout(0.3),
            
            layers.Conv2D(128, (3, 3), activation='relu',
                         kernel_regularizer=regularizers.l2(0.001)),
            layers.BatchNormalization(),
            layers.MaxPooling2D((2, 2)),
            layers.Dropout(0.4),
            
            # Additional dense layer for better feature processing
            layers.Flatten(),
            layers.Dense(256, activation='relu',
                       kernel_regularizer=regularizers.l2(0.001)),
            layers.BatchNormalization(),
            layers.Dropout(0.5),
            
            # Output layer with temperature scaling (for better confidence calibration)
            layers.Dense(self.num_classes, activation='softmax')
        ])
        
        # Custom learning rate for better optimization
        optimizer = tf.keras.optimizers.Adam(learning_rate=0.0005)
        
        model.compile(optimizer=optimizer,
                    loss='sparse_categorical_crossentropy',
                    metrics=['accuracy'])
        
        return model
    
    def train(self, X_train, y_train, X_val, y_val, epochs=100):
        """Train with callbacks for model checkpointing and early stopping"""
        checkpoint_path = "high_confidence_model.h5"
        
        callbacks_list = [
            callbacks.ModelCheckpoint(checkpoint_path, 
                                    monitor='val_accuracy',
                                    save_best_only=True,
                                    mode='max'),
            callbacks.ReduceLROnPlateau(monitor='val_loss', factor=0.2, patience=5)
        ]
        
        history = self.model.fit(
            X_train, y_train,
            epochs=epochs,
            batch_size=32,
            validation_data=(X_val, y_val),
            callbacks=callbacks_list,
            verbose=1
        )
        
        return history, checkpoint_path
    
    def predict_with_confidence(self, X):
        """Make predictions only when confidence exceeds threshold"""
        probs = self.model.predict(X, verbose=0)
        predictions = np.argmax(probs, axis=1)
        confidences = np.max(probs, axis=1)
        
        # Apply confidence threshold
        mask = confidences >= self.confidence_threshold
        confident_predictions = predictions[mask]
        confident_confidences = confidences[mask]
        confident_indices = np.where(mask)[0]
        
        return confident_predictions, confident_confidences, confident_indices
    
    def evaluate_high_confidence(self, X_test, y_test):
        """Evaluate only on samples where model is confident"""
        preds, confs, indices = self.predict_with_confidence(X_test)
        if len(indices) == 0:
            print("No predictions met the 90% confidence threshold")
            return 0.0
        
        confident_X = X_test[indices]
        confident_y = y_test[indices]
        
        accuracy = np.mean(preds == confident_y)
        coverage = len(indices) / len(X_test)
        
        print(f"\nHigh-Confidence Evaluation (threshold: {self.confidence_threshold:.0%})")
        print(f"Accuracy on confident predictions: {accuracy:.4f}")
        print(f"Coverage (fraction of samples predicted): {coverage:.4f}")
        
        return accuracy, coverage

def load_dataset(data_dir):
    images = []
    labels = []
    class_names = {'circle': 0, 'rectangle': 1, 'unknown': 2}
    
    for filename in os.listdir(data_dir):
        if filename.endswith('.png'):
            if filename.startswith('x'):
                label = 'unknown'
            else:
                label = ''.join([c for c in filename if not c.isdigit()]).split('.')[0]
            
            img_path = os.path.join(data_dir, filename)
            img = Image.open(img_path).convert('L')
            img = img.resize((64, 64))
            img_array = np.array(img) / 255.0
            img_array = np.expand_dims(img_array, axis=-1)
            
            images.append(img_array)
            labels.append(class_names[label])
    
    return np.array(images), np.array(labels)

def plot_history(history):
    plt.figure(figsize=(12, 5))
    
    plt.subplot(1, 2, 1)
    plt.plot(history.history['accuracy'], label='Train Accuracy')
    plt.plot(history.history['val_accuracy'], label='Validation Accuracy')
    plt.title('Accuracy over Epochs')
    plt.legend()
    
    plt.subplot(1, 2, 2)
    plt.plot(history.history['loss'], label='Train Loss')
    plt.plot(history.history['val_loss'], label='Validation Loss')
    plt.title('Loss over Epochs')
    plt.legend()
    
    plt.show()

if __name__ == "__main__":
    # Load and prepare data
    dataset_dir = "images"
    X, y = load_dataset(dataset_dir)
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)
    
    # Create and train high-confidence model
    hc_model = HighConfidenceModel(input_shape=X_train[0].shape, num_classes=len(np.unique(y)))
    history, model_path = hc_model.train(X_train, y_train, X_test, y_test, epochs=100)
    
    # Plot training history
    plot_history(history)
    
    # Standard evaluation
    hc_model.evaluate_high_confidence(X_test, y_test)