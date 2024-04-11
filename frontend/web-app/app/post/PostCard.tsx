import {Post} from "@/types";
import Link from "next/link";
import CarImage from "@/app/auctions/CarImage";
import Image from "next/image";


type Props = {
  post: Post
}

export default function PostCard({post}: Props) {
  const excerpt = (content: string) => content.substring(0, 100) + '...';

  return (
      <Link href={`/post/details/${post.guid}`}>
        <a className='block shadow-lg rounded-lg overflow-hidden'>
          <Image src={post.imageUrl} alt={post.title} width={250} height={250} className='w-full h-48 object-cover' />
          <div className='p-4'>
            <h3 className='text-lg font-semibold text-gray-800'>{post.title}</h3>
            <p className='text-gray-600'>{excerpt(post.content)}</p>
          </div>
        </a>
      </Link>
  )
}
