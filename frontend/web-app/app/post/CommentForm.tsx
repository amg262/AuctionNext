"use client";

import React from "react";
import {Label, Textarea} from "flowbite-react";

type Props = {
  comment: string;
  isSubmitting: boolean;
  handleChange: (event: React.ChangeEvent<HTMLTextAreaElement>) => void;
  handleKeyDown: (event: React.KeyboardEvent<HTMLTextAreaElement>) => void;
  handleSubmit: (event: React.FormEvent<HTMLFormElement>) => Promise<void>;
};
export default function CommentForm({comment, isSubmitting, handleChange, handleKeyDown, handleSubmit}: Props) {

  return (
      <div className="max-w-lg">
        <form onSubmit={handleSubmit}>
          <div className="mb-2 block">
            <Label htmlFor="comment" value="Your message"/>
          </div>
          <Textarea
              id="comment"
              placeholder="Add a comment..."
              className="w-full p-2 border rounded"
              value={comment}
              onChange={handleChange}
              onKeyDown={handleKeyDown}
              disabled={isSubmitting}
              rows={4}
          ></Textarea>
          <button type="submit" disabled={isSubmitting}
                  className="mt-2 bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded">
            {isSubmitting ? 'Posting...' : 'Post Comment'}
          </button>
        </form>
      </div>
  );
}