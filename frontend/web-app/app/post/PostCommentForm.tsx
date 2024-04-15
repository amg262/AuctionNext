"use client";

import {SetStateAction, useState} from "react";
import {createComment} from "@/app/actions/auctionActions";

export default function PostCommentForm({postId}: { postId: string; }) {
  const [loading, setLoading] = useState(false);
  const [comment, setComment] = useState('');


  const handleChange = (event: { target: { value: SetStateAction<string>; }; }) => {
    setComment(event.target.value);
  };

  const handleSubmit = async (event: { preventDefault: () => void; }) => {
    event.preventDefault();

    console.log('postId', postId)
    console.log('comment', comment)

    await createComment(postId, comment);
    setComment('');

    if (comment.trim()) {
      // onSubmit(postId, comment);

    }
  };

  return (
      <div className='mt-4'>
        <form onSubmit={handleSubmit}>
        <textarea
            className="w-full p-2 border rounded"
            placeholder="Add a comment..."
            value={comment}
            onChange={handleChange}
            rows={4}
        ></textarea>
          <button type="submit" className="mt-2 bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded">
            Post Comment
          </button>
        </form>
      </div>
  );
}