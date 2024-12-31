import React from 'react';

const BookReviewCard = ({ review }) => {
  const [userName, reviewText] = review.split(': ');

  return (
    <div className="bg-gray-100 p-4 rounded-lg shadow-md">
      <p className="font-semibold text-gray-800">{userName}</p>
      <p className="text-gray-600">{reviewText}</p>
    </div>
  );
};

export default BookReviewCard;