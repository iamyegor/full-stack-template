import LanguageSwitcher from "@/features/languages/components/LanguageSwitcher";
import Language from "@/features/languages/types/Language";
import fetchTodoLists from "@/features/todos/api/fetchTodoLists";
import { Link } from "@/i18n/routing";
import { Infinity, ListTodo, LogIn } from "lucide-react";
import { getLocale } from "next-intl/server";

export default async function HomePage() {
    const locale = (await getLocale()) as Language;
    const { data } = await fetchTodoLists(locale);
    const [finite, infinite] = data.todoLists;

    return (
        <div className="bg-gradient-to-br from-purple-100 to-indigo-100 font-sans">
            <LanguageSwitcher />
            <Link
                href="/signin"
                className="absolute right-5 top-5 bg-blue-600 p-1 rounded-full w-11 h-11 flex items-center justify-center"
            >
                <LogIn className="text-white mr-1" />
            </Link>
            <div className="container mx-auto min-h-screen flex flex-col md:flex-row justify-center items-center py-12 gap-8">
                <div className="w-full md:w-1/2 max-w-md p-8 bg-white border border-purple-300 rounded-3xl duration-300 flex flex-col justify-between items-center">
                    <div className="flex flex-col items-center">
                        <ListTodo className="w-24 h-24 text-purple-600 mb-3" />
                        <h1 className="text-[32px] lg:text-[45px] font-bold text-purple-800 mb-4 text-center leading-[1.1]">
                            {finite.name}
                        </h1>
                        <p className="text-purple-600 mb-12 text-center">{finite.description}</p>
                    </div>
                    <Link
                        href={`/${finite.slug}`}
                        className="bg-purple-600 hover:bg-purple-700 text-white font-bold py-3 px-6 rounded-full text-lg lg:text-xl transition-colors duration-300"
                    >
                        {finite.buttonText}
                    </Link>
                </div>
                <div className="w-full md:w-1/2 max-w-md p-8 bg-white border border-indigo-300 rounded-3xl duration-300 flex flex-col justify-between items-center">
                    <div className="flex flex-col items-center">
                        <Infinity className="w-24 h-24 text-indigo-600 mb-3" />
                        <h1 className="text-[32px] lg:text-[45px] font-bold text-indigo-800 mb-4 leading-[1.1] text-center">
                            {infinite.name}
                        </h1>
                        <p className="text-indigo-600 mb-12 text-center">{infinite.description}</p>
                    </div>
                    <Link
                        href={`/${infinite.slug}`}
                        className="bg-indigo-600 hover:bg-indigo-700 text-white font-bold py-3 px-6 rounded-full text-lg lg:text-xl transition-colors duration-300"
                    >
                        {infinite.buttonText}
                    </Link>
                </div>
            </div>
        </div>
    );
}
