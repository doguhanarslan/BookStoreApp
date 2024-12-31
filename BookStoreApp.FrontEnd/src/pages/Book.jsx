import React from 'react';
import { useLocation } from 'react-router-dom';
import BookDetails from '../components/BookDetails';

function Book() {
  const location = useLocation();
  const { book } = location.state || {};

  return (
    <div>
      <BookDetails book={book} />
    </div>
  );
}

export default Book;