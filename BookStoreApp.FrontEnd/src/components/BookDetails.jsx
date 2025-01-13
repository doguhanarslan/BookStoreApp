import React, { useState, useContext, useEffect } from "react";
import axios from "axios";
import { StoreContext } from "../context/StoreContext";
import { FaRegStar, FaStar, FaStarHalfAlt } from "react-icons/fa";

const BookDetails = ({ book }) => {
  const { user } = useContext(StoreContext); // Kullanıcı bilgilerini context'ten alın
  const [reviews, setReviews] = useState([]);
  const [newReview, setNewReview] = useState("");
  const [rating, setRating] = useState(0);
  const [error, setError] = useState("");

  if (!book) {
    return <div>Book not found</div>;
  }

  const fetchReviewsByBookId = async () => {
    try {
      const response = await axios.get(
        `https://localhost:7118/getReviewsByBookId?bookId=${book.bookId}`
      );
      setReviews(response.data);
      console.log(response.data);
    } catch (error) {
      console.error("Error fetching reviews:", error);
    }
  };

  useEffect(() => {
    fetchReviewsByBookId();
  }, [book]);

  const handleReviewSubmit = async (e) => {
    e.preventDefault();
    if (!newReview) {
      setError("Review text is required.");
      return;
    }
    const reviewDate = new Date().toISOString();
    try {
      console.log(reviews);
      setReviews([
        ...reviews,
        {
          reviewText: newReview,
          userName: user.userName,
          rating: rating,
          reviewDate: reviewDate,
        },
      ]);
      setNewReview("");
      setRating(0);
      setError("");
      await axios.post(`https://localhost:7118/addReview`, {
        BookId: book.bookId,
        UserId: user.id,
        UserName: user.userName, // Kullanıcı ID'sini context'ten alın
        ReviewText: newReview,
        Rating: rating,
        ReviewDate: reviewDate,
      });
    } catch (error) {
      console.error("Error submitting review:", error);
      setError("Error submitting review. Please try again.");
    }
  };

  const renderStars = (rating) => {
    const stars = [];
    for (let i = 1; i <= 5; i++) {
      if (i <= rating) {
        stars.push(<FaStar key={i} className="text-yellow-500 w-4" />);
      } else if (i === Math.ceil(rating) && !Number.isInteger(rating)) {
        stars.push(<FaStarHalfAlt key={i} className="text-yellow-500 w-4" />);
      } else {
        stars.push(<FaRegStar key={i} className="text-yellow-500 w-4" />);
      }
    }
    return stars;
  };
  return (
    <div className="max-w-4xl mx-auto p-6 bg-white shadow-md rounded-lg">
      <div className="flex flex-col md:flex-row">
        <img
          className="w-full md:w-1/3 object-cover rounded-lg"
          src={book.bookImage}
          alt={book.bookTitle}
          style={{ maxHeight: "400px" }} // Görselin tam görünmesi için maxHeight ayarı
        />
        <div className="md:ml-6 mt-4 md:mt-0">
          <h1 className="text-3xl font-bold text-gray-800">{book.bookTitle}</h1>
          <p className="text-gray-600 mt-2">{book.bookDescription}</p>
          <p className="text-gray-800 mt-4">
            <span className="font-semibold">Author:</span> {book.authorName}
          </p>
          <p className="text-gray-800 mt-2">
            <span className="font-semibold">Price:</span> ${book.bookPrice}
          </p>
          <p className="text-gray-800 mt-2">
            <div className="flex flex-row items-center justify-between">
              <span className="font-semibold">Rating:</span> {book.bookRate}{" "}
              {renderStars(book.bookRate)} ({book.bookReviews.length} reviews)
            </div>
          </p>
        </div>
      </div>
      <div className="mt-6">
        <h2 className="text-2xl font-bold text-gray-800">Reviews</h2>
        <ul className="mt-4 space-y-2">
          {reviews.map((review, index) => (
            <li key={index} className="bg-gray-100 p-4 rounded-lg shadow-md">
              <p className="font-semibold text-gray-800">{review.userName}</p>
              <p className="text-gray-600">{review.reviewText}</p>
              <p className="text-gray-600 flex flex-row items-center gap-1">
                Rating: {review.rating} {renderStars(book.bookRate)}
              </p>
              <p className="text-gray-600">
                Date: {new Date(review.reviewDate).toLocaleDateString()}
              </p>
            </li>
          ))}
        </ul>
      </div>
      {user && (
        <div className="mt-6">
          <h2 className="text-2xl font-bold text-gray-800">Add a Review</h2>
          <form onSubmit={handleReviewSubmit} className="mt-4">
            {error && <p className="text-red-500 mb-4">{error}</p>}
            <div className="mb-4">
              <label
                className="block text-gray-700 text-sm font-bold mb-2"
                htmlFor="rating"
              >
                Rating
              </label>
              <select
                id="rating"
                value={rating}
                onChange={(e) => setRating(parseInt(e.target.value))}
                className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
              >
                <option value={0}>Select a rating</option>
                <option value={1}>1</option>
                <option value={2}>2</option>
                <option value={3}>3</option>
                <option value={4}>4</option>
                <option value={5}>5</option>
              </select>
            </div>
            <div className="mb-4">
              <label
                className="block text-gray-700 text-sm font-bold mb-2"
                htmlFor="review"
              >
                Review
              </label>
              <textarea
                id="review"
                value={newReview}
                onChange={(e) => setNewReview(e.target.value)}
                className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                rows="4"
              ></textarea>
            </div>
            <div className="flex items-center justify-between">
              <button
                type="submit"
                className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
              >
                Submit Review
              </button>
            </div>
          </form>
        </div>
      )}
    </div>
  );
};

export default BookDetails;
