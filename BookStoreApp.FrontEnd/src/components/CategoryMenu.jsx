import React, { useContext, useEffect, useState } from "react";
import { StoreContext } from "../context/StoreContext";

const CategoryMenu = ({ onCategoryClick }) => {
  const [categories, setCategories] = useState([]);
    const {fetchCategories} = useContext(StoreContext);
  useEffect(() => {
    const loadCategories = async () => {
      const data = await fetchCategories();
      setCategories(data);
    };
    loadCategories();
  }, []);

  return (
    <div className="flex space-x-4 p-4 bg-gray-100">
      {categories.map((category) => (
        <button
          key={category.categoryId}
          onClick={() => onCategoryClick(category)}
          className="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600"
        >
          {category.categoryName}
        </button>
      ))}
    </div>
  );
};

export default CategoryMenu;
