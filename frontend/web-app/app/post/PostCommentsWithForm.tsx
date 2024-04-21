'use client';

import React, {useEffect} from "react";
import {Post, PostComment} from "@/types";
import {createComment, getPostComments} from "@/app/actions/auctionActions";

type Props = {
  username?: string,
  post: Post
}

export default function PostCommentsWithForm({username, post}: Props) {
  const [postComments, setPostComments] = React.useState<PostComment[]>([]);
  const [error, setError] = React.useState<any>(null);
  const [comment, setComment] = React.useState('');
  const [isSubmitting, setIsSubmitting] = React.useState(false);

  const handleChange = (event: { target: { value: React.SetStateAction<string>; }; }) => {
    setComment(event.target.value);
  };

  const handleSubmit = async (event: { preventDefault: () => void; }) => {
    event.preventDefault();
    setIsSubmitting(true);
    try {
      const response = await createComment(post.id, {content: comment, userId: username});
      console.log('Comment created', response);
      setPostComments([...postComments, {postId: post.id, content: comment, userId: username}]);
      setComment('');
      setIsSubmitting(false);
    } catch (error) {
      console.error('Failed to post comment', error);
    }
  }

  useEffect(() => {

    async function fetchPostComments() {
      try {
        const comments = await getPostComments(post.id);
        setPostComments(comments);
      } catch (error) {
        setError(error);
        console.error('Failed to fetch post comments', error);
      }
    }

    fetchPostComments().then(r => r).catch(e => e);

  }, [post.id, comment, postComments.length]);


  const handleKeyDown = (event: React.KeyboardEvent<HTMLTextAreaElement>) => {
    // Check if the Enter key was pressed and also ensure that the Shift key was NOT held down.
    if (event.key === 'Enter' && !event.shiftKey) {
      event.preventDefault();  // Prevent the default action to avoid a new line in textarea
      handleSubmit(event).then(r => r).catch(error => error);
    }
  };


  if (error) {
    return <div>Error: {error}</div>;
  }

  return (
      <div>

        <div className='mt-4'>
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
        <div className='mt-6'>
          <h2 className='text-2xl font-bold text-gray-800'>Comments:</h2>
          {postComments.length > 0 ? (
              <ul>
                {postComments.map((comment, index) => (
                    <li key={index} className="bg-gray-100 rounded p-3 my-2">
                      <p>{comment.content}</p>
                      <p className="text-gray-500 text-sm">Posted by: {comment.userId}</p>
                    </li>
                ))}
              </ul>
          ) : (
              <p>No comments yet.</p>
          )}
        </div>
      </div>
  );
}