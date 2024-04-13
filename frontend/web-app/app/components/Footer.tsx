import Image from "next/image";

export default function Footer() {
  return (
      <div>
        <footer className="bg-white rounded-lg shadow dark:bg-gray-900 m-4">
          <div className="w-full max-w-screen-xl mx-auto p-4 md:py-8">
            <div className="sm:flex sm:items-center sm:justify-between">
              <a href="https://github.com/amg262/"
                 className="flex items-center mb-4 sm:mb-0 space-x-3 rtl:space-x-reverse">
                <Image src="/payment.png" height={32} className="h-8" alt="Flowbite Logo"/>
                <span
                    className="self-center text-2xl font-semibold whitespace-nowrap dark:text-white">AuctionNext</span>
              </a>
              <span className="block text-sm text-gray-500 sm:text-center dark:text-gray-400">© 2024 <a
                  href="https://flowbite.com/"
                  className="hover:underline">Milwaukee Software™</a>. All Rights Reserved.</span>
              <ul className="flex flex-wrap items-center mb-6 text-sm font-medium text-gray-500 sm:mb-0 dark:text-gray-400">
                <li>
                  <a href="https://github.com/amg262/AuctionNext" target="_blank"
                     className="hover:underline me-4 md:me-6">Github</a>
                </li>
                <li>
                  <a href="https://www.linkedin.com/in/andrewmgunn/" target="_blank"
                     className="hover:underline">LinkedIn</a>
                </li>
              </ul>
            </div>
            {/*<hr className="my-6 border-gray-200 sm:mx-auto dark:border-gray-700 lg:my-8"/>*/}
            {/*<span className="block text-sm text-gray-500 sm:text-center dark:text-gray-400">© 2024 <a*/}
            {/*    href="https://flowbite.com/"*/}
            {/*    className="hover:underline">Milwaukee Software™</a>. All Rights Reserved.</span>*/}
          </div>
        </footer>
      </div>
  );
}