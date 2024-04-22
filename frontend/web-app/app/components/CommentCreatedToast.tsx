import {PostComment} from '@/types'
import Image from 'next/image'
import Link from 'next/link'
import React from 'react'

type Props = {
  postComment: PostComment
}

export default function CommentCreatedToast({postComment}: Props) {
  console.log('postComment - toast', postComment)
  return (
      <Link href={`/post/details/${postComment.postId}`} className='flex flex-col items-center'>
        <div className='flex flex-row items-center gap-2'>
          <Image
              src="/comment.png"
              alt='image'
              height={80}
              width={80}
              className='rounded-lg w-auto h-auto'
          />
          <span>New Comment by {postComment.userId}</span>
        </div>
      </Link>
  )
}
