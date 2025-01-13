import React, { useState } from 'react';
import axios from 'axios';

const AddReviewForm = ({ bookId, userId, onReviewAdded }) => {
    const [reviewText, setReviewText] = useState('');
    const [rating, setRating] = useState(0);
    const [error, setError] = useState('');

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!reviewText) {
            setError('Review text is required.');
            return;
        }
        try {
            const response = await axios.post('https://localhost:7118/addReview', {
                bookId,
                userId,
                reviewText,
                rating,
                reviewDate: new Date().toISOString()
            });
            console.log('Review added:', response.data);
            setReviewText('');
            setRating(0);
            setError('');
            if (onReviewAdded) {
                onReviewAdded(response.data);
            }
        } catch (error) {
            console.error('Error adding review:', error.response?.data || error.message);
            setError('Error adding review. Please try again.');
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            {error && <p className="text-red-500 mb-4">{error}</p>}
            <div>
                <label>Review Text:</label>
                <textarea
                    value={reviewText}
                    onChange={(e) => setReviewText(e.target.value)}
                    required
                />
            </div>
            <div>
                <label>Rating:</label>
                <select
                    value={rating}
                    onChange={(e) => setRating(parseInt(e.target.value))}
                    required
                >
                    <option value={0}>Select a rating</option>
                    <option value={1}>1</option>
                    <option value={2}>2</option>
                    <option value={3}>3</option>
                    <option value={4}>4</option>
                    <option value={5}>5</option>
                </select>
            </div>
            <button type="submit">Submit Review</button>
        </form>
    );
};

export default AddReviewForm;