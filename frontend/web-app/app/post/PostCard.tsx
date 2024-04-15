'use client';

import {Post} from "@/types";
import Link from "next/link";
import Image from "next/image";
import {Button} from "flowbite-react";
import {useState} from "react";


type Props = {
  post: Post
}

export default function PostCard({post}: Props) {
  const excerpt = (content: string) => content.substring(0, 150) + '...';
  const [isLoading, setLoading] = useState(true);

  return (
      <Link href={`/post/details/${post.id}`}>
        <a className='block shadow-lg rounded-lg overflow-hidden'>
          <Image src={post.imageUrl} alt={post.title}
                 sizes='(max-width:768px) 100vw, (max-width: 1200px) 50vw, 25vw'
                 onLoadingComplete={() => setLoading(false)}
                 className={`
                object-cover
                group-hover:opacity-75
                duration-700
                ease-in-out
                ${isLoading ? 'grayscale blur-2xl scale-110' : 'grayscale-0 blur-0 scale-100'}
                
            `}
          />
          <div className='p-4'>
            <h3 className='text-lg font-semibold text-gray-800'>{post.title}</h3>
            <p className='text-gray-600'>{excerpt(post.content)}</p>
            <Button className='mt-4'>
              <Link href={`/post/details/${post.id}`}>
                Read more
              </Link>
            </Button>
          </div>
        </a>
      </Link>
  )
}
