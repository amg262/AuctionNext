"use client";

import React from "react";

type Props = {
  comment: string;
  isSubmitting: boolean;
  handleChange: (event: React.ChangeEvent<HTMLTextAreaElement>) => void;
  handleKeyDown: (event: React.KeyboardEvent<HTMLTextAreaElement>) => void;
  handleSubmit: (event: React.FormEvent<HTMLFormElement>) => Promise<void>;
};
export default function CommentForm({comment, isSubmitting, handleChange, handleKeyDown, handleSubmit}: Props) {

  return (
      <div>
        <form onSubmit={handleSubmit}>
            <textarea
                className="w-full p-2 border rounded"
                placeholder="Add a comment..."
                value={comment}
                onChange={handleChange}
                onKeyDown={handleKeyDown}
                disabled={isSubmitting}
                rows={4}
            ></textarea>
          <button type="submit" disabled={isSubmitting}
                  className="mt-2 bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded">
            {isSubmitting ? 'Posting...' : 'Post Comment'}
          </button>
        </form>
      </div>
  );
}