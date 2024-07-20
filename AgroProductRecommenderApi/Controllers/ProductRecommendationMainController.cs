using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductRecommendationMainController : ControllerBase
    {
        private readonly AgroProductRecommenderDBContext _context;
        private readonly MLContext _mlContext;
        private PredictionEngine<ProductRating, ProductPrediction> _predictionEngine;

        public ProductRecommendationMainController(AgroProductRecommenderDBContext context)
        {
            _context = context;
            _mlContext = new MLContext();
            TrainModel();
        }

        private void TrainModel()
        {
            // Load data from database
            var ratings = _context.ProductRatings.Select(r => new ProductRating
            {
                UserId = r.UserId,
                ProductId = r.ProductId,
                Label = r.Label//rating
            }).ToList();

            IDataView data = _mlContext.Data.LoadFromEnumerable(ratings);

            // Split data into training and test sets
            var trainTestData = _mlContext.Data.TrainTestSplit(data, testFraction: 0.2);
            var trainingData = trainTestData.TrainSet;
            var testData = trainTestData.TestSet;

            // Configure matrix factorization options
            var options = new MatrixFactorizationTrainer.Options
            {
                MatrixColumnIndexColumnName = nameof(ProductRating.ProductId),
                MatrixRowIndexColumnName = nameof(ProductRating.UserId),
                LabelColumnName = nameof(ProductRating.Label),
                NumberOfIterations = 20,
                ApproximationRank = 100
            };

            // Train the model
            var estimator = _mlContext.Recommendation().Trainers.MatrixFactorization(options);
            var model = estimator.Fit(trainingData);

            // Evaluate the model
            var prediction = model.Transform(testData);
            var metrics = _mlContext.Regression.Evaluate(prediction, labelColumnName: nameof(ProductRating.Label), scoreColumnName: nameof(ProductPrediction.Score));
            Console.WriteLine($"RMSE: {metrics.RootMeanSquaredError}");

            // Create prediction engine
            _predictionEngine = _mlContext.Model.CreatePredictionEngine<ProductRating, ProductPrediction>(model);
        }

        [HttpGet("GetRecommendations/{userId}")]
        public ActionResult<IEnumerable<ProductRecommendation>> GetRecommendations(int userId)
        {
            var recommendations = new List<ProductRecommendation>();

            // Get all products
            var products = _context.Products.ToList();

            // Generate recommendations
            foreach (var product in products)
            {
                var testInput = new ProductRating { UserId = userId, ProductId = product.Id };
                var productPrediction = _predictionEngine.Predict(testInput);
                recommendations.Add(new ProductRecommendation
                {
                    ProductId = product.Id,
                    Score = productPrediction.Score
                });
            }

            // Return top 10 recommendations
            return Ok(recommendations.OrderByDescending(r => r.Score).Take(10));
        }
    }

    public class ProductRating
    {
        public float UserId { get; set; }
        public float ProductId { get; set; }
        public float Label { get; set; } // Rating
    }

    public class ProductPrediction
    {
        public float Score { get; set; }
    }

    public class ProductRecommendation
    {
        public int ProductId { get; set; }
        public float Score { get; set; }
    }
}
